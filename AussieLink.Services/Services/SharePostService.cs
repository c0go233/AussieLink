using AussieLink.Contracts.Enums;
using AussieLink.Contracts.Models;
using AussieLink.Contracts.Models.PostModels.SharePostModels;
using AussieLink.Contracts.Responses;
using AussieLink.Contracts.Services;
using AussieLink.Contracts.UnitOfWork;
using AussieLink.Contracts.ViewModels;
using AussieLink.Contracts.ViewModels.PostViewModels.SharePostViewModels;
using AussieLink.Services.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AussieLink.Services.Services
{
    public class SharePostService : BasePostService, ISharePostService
    {
        private readonly ISharePostUnitOfWork SharePostUnitOfWork;
        private readonly int PictureNumberLimit = 5;
        private readonly int PictureSizeLimit = 2;

        public SharePostService(ISharePostUnitOfWork sharePostUnitOfWork, IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.SharePostUnitOfWork = sharePostUnitOfWork;
        }

        public bool SetSharePostVMForEdit(SharePostVM vm, string userEmail)
        {
            Guid userId = base.GetUserIdFromUserEmail(userEmail);
            if (userId == Guid.Empty)
                return false;

            var sharePost = GetSharePostForUserId(vm.PostId, userId, true);
            if (sharePost == null)
                return false;

            PopulateCategories(vm);
            Mapper.SharePostToSharePostVM(sharePost, vm);
            if (sharePost.Pictures != null)
                vm.SavedPictures = Mapper.PicturesToPictureVMs(sharePost.Pictures, new List<PictureVM>());

            return true;
        }

        private SharePost GetSharePostForUserId(string postId, Guid userIdInGuid, bool isFullPost)
        {
            int decryptedPostId;
            if (!base.GetDecryptedPostId(postId, out decryptedPostId))
                return null;

            var post = isFullPost ? SharePostUnitOfWork.SharePosts.GetFullSharePost(decryptedPostId)
                : SharePostUnitOfWork.SharePosts.SingleOrDefault(m => m.PostId == decryptedPostId);

            if (post == null)
                return null;

            if (!post.IsOwnedByUserId(userIdInGuid))
                return null;

            if (post.Cancel)
                return null;

            return post;
        }

        public SharePostVM PopulateCategories(SharePostVM vm)
        {
            vm.SharePostCategories.Places = SharePostUnitOfWork.Places.GetAll();
            vm.SharePostCategories.SydneySuburbs = Mapper.SuburbToSuburbVM(SharePostUnitOfWork.Suburbs.WhereBy(m => m.PlaceId == (int)PlaceId.Sydney), new List<SuburbVM>());
            vm.SharePostCategories.MelbourneSuburbs = Mapper.SuburbToSuburbVM(SharePostUnitOfWork.Suburbs.WhereBy(m => m.PlaceId == (int)PlaceId.Melbourne), new List<SuburbVM>());
            vm.SharePostCategories.BrisbaneSuburbs = Mapper.SuburbToSuburbVM(SharePostUnitOfWork.Suburbs.WhereBy(m => m.PlaceId == (int)PlaceId.Brisbane), new List<SuburbVM>());
            vm.SharePostCategories.AdelaideSuburbs = Mapper.SuburbToSuburbVM(SharePostUnitOfWork.Suburbs.WhereBy(m => m.PlaceId == (int)PlaceId.Adelaide), new List<SuburbVM>());
            vm.SharePostCategories.PerthSuburbs = Mapper.SuburbToSuburbVM(SharePostUnitOfWork.Suburbs.WhereBy(m => m.PlaceId == (int)PlaceId.Perth), new List<SuburbVM>());
            vm.SharePostCategories.Genders = SharePostUnitOfWork.Genders.GetAll();
            vm.SharePostCategories.ShareTypes = SharePostUnitOfWork.ShareTypes.GetAll();
            return vm;
        }

        public SaveSharePostResponse SaveSharePost(SharePostVM vm, IEnumerable<HttpPostedFileBase> unsavedPictures, string userEmail)
        {
            Guid userId = base.GetUserIdFromUserEmail(userEmail);
            if (userId == Guid.Empty)
                return new SaveSharePostResponse(false, ErrorCode.BADREQUEST);

            UrlDecodeSharePostVM(vm);

            if (vm.PostId == null)
                return CreateSharePost(vm, userId, unsavedPictures);
            else
                return UpdateSharePost(vm, userId, unsavedPictures);
        }

        private SaveSharePostResponse UpdateSharePost(SharePostVM vm, Guid userId, IEnumerable<HttpPostedFileBase> unsavedPictures)
        {
            var sharePost = GetSharePostForUserId(vm.PostId, userId, false);
            if (sharePost == null)
                return new SaveSharePostResponse(false, ErrorCode.BADREQUEST);

            var address = SharePostUnitOfWork.Addresses.SingleOrDefault(m => m.PostId == sharePost.PostId);
            if (address == null)
                return new SaveSharePostResponse(false, ErrorCode.BADREQUEST);

            if (!MapSharePostVMToSharePost(vm, sharePost, false) || !MapSharePostVMToAddress(vm, address))
                return new SaveSharePostResponse(false, ErrorCode.BADREQUEST);

            try { UpdateSavedPictures(vm.SavedPictures, sharePost.PostId); }
            catch (Exception e) { return new SaveSharePostResponse(false, ErrorCode.BADREQUEST); }

            SharePostUnitOfWork.Complete();

            var isPictureValid = ValidatePictrues(vm.PostId, unsavedPictures);
            if (!isPictureValid)
                return new SaveSharePostResponse(false, ErrorCode.BADREQUEST);

            SavePictures(unsavedPictures, sharePost.PostId);

            SharePostUnitOfWork.Complete();

            return null;
        }

        private void UpdateSavedPictures(IEnumerable<PictureVM> savedPictures, int postId)
        {
            var savedPicturesInDb = SharePostUnitOfWork.Pictures.WhereBy(m => m.PostId == postId);
            if (savedPictures == null)
            {
                SharePostUnitOfWork.Pictures.RemoveRage(savedPicturesInDb);
            }
            else if (savedPictures != null && savedPicturesInDb !=  null)
            {
                foreach(var pictureInDb in savedPicturesInDb)
                {
                    if (!HasPictureIdInSavedPictures(savedPictures, pictureInDb.PictureId))
                        SharePostUnitOfWork.Pictures.Remove(pictureInDb);
                }
            }
        }



        private bool HasPictureIdInSavedPictures(IEnumerable<PictureVM> savedPictures, int pictureInDbId)
        {
            foreach(var picture in savedPictures)
            {
                int decryptedPictureId;
                if (!base.GetDecryptedPostId(picture.PictureId, out decryptedPictureId))
                    throw new Exception("invlid picture id");

                if (pictureInDbId == decryptedPictureId)
                    return true;
            }
            return false;
        }

        private SaveSharePostResponse CreateSharePost(SharePostVM vm, Guid userId, IEnumerable<HttpPostedFileBase> unsavedPictures)
        {
            var isPictureValid = ValidatePictrues(vm.PostId, unsavedPictures);
            if (!isPictureValid)
                return new SaveSharePostResponse(false, ErrorCode.BADREQUEST);

            var sharePost = new SharePost() { UserId = userId };
            var address = new Address();

            if (!MapSharePostVMToSharePost(vm, sharePost, true) || !MapSharePostVMToAddress(vm, address))
                return new SaveSharePostResponse(false, ErrorCode.BADREQUEST);

            SharePostUnitOfWork.SharePosts.Add(sharePost);

            address.PostId = sharePost.PostId;

            SharePostUnitOfWork.Addresses.Add(address);
            SavePictures(unsavedPictures, sharePost.PostId);

            SharePostUnitOfWork.Complete();

            var encryptedPostId = Encryptor.GetEncryptedString(sharePost.PostId.ToString());

            return new SaveSharePostResponse(true, encryptedPostId);
        }

        private void SavePictures(IEnumerable<HttpPostedFileBase> unsavedPictures, int postId)
        {
            if (unsavedPictures != null)
            {
                foreach (var picture in unsavedPictures)
                {
                    if (picture.ContentLength > 0)
                    {
                        var data = new byte[picture.ContentLength];
                        picture.InputStream.Read(data, 0, picture.ContentLength);
                        var pictureModel = new Picture(postId, data, picture.FileName, picture.ContentLength, picture.ContentType);
                        SharePostUnitOfWork.Pictures.Add(pictureModel);
                    }
                }
            }
        }

        private bool ValidatePictrues(string postId, IEnumerable<HttpPostedFileBase> unsavedPictures)
        {
            if (!ValidatePictureFileType(unsavedPictures))
                return false;

            var totalPitureCount = unsavedPictures.Count();
            var totalPitureSize = GetTotalSizeFromUnsavedPictures(unsavedPictures);

            if (postId != null)
            {
                int decryptedPostId;
                if (!base.GetDecryptedPostId(postId, out decryptedPostId))
                    return false;

                var savedPictures = SharePostUnitOfWork.Pictures.WhereBy(m => m.PostId == decryptedPostId);
                if (savedPictures != null)
                {
                    totalPitureCount += savedPictures.Count();
                    totalPitureSize += GetTotalSizeFromSavedPictures(savedPictures);
                }
            }

            if (totalPitureCount > PictureNumberLimit || totalPitureSize > PictureSizeLimit)
                return false;

            return true;
        }

        private bool ValidatePictureFileType(IEnumerable<HttpPostedFileBase> unsavedPictures)
        {
            foreach(var picture in unsavedPictures)
            {
                if (!picture.ContentType.Contains("image"))
                    return false;
            }
            return true;
        }

        private double GetTotalSizeFromUnsavedPictures(IEnumerable<HttpPostedFileBase> unsavedPictures)
        {
            double totalSize = 0;
            foreach(var picture in unsavedPictures)
            {
                totalSize += ConvertToSizeInMB(picture.ContentLength);
            }
            return totalSize;
        }

        private double GetTotalSizeFromSavedPictures(IEnumerable<Picture> savedPictures)
        {
            double totalSize = 0;
            foreach (var picture in savedPictures)
            {
                totalSize += ConvertToSizeInMB(picture.Size);
            }
            return totalSize;
        }

        private double ConvertToSizeInMB(int size)
        {
            var sizeInMB = (size / (double)(1024 * 1024));
            return sizeInMB;
        }

        private bool MapSharePostVMToAddress(SharePostVM vm, Address address)
        {
            if (SharePostUnitOfWork.Places.FindById(vm.PlaceId) == null)
                return false;

            var suburb = SharePostUnitOfWork.Suburbs.SingleOrDefault(m => (m.PlaceId == vm.PlaceId) && (m.Name == vm.Suburb));
            if (suburb == null)
                return false;

            address.PlaceId = vm.PlaceId;
            address.SuburbId = suburb.SuburbId;
            return true;
        }

        private bool MapSharePostVMToSharePost(SharePostVM vm, SharePost sharePost, bool isCreation)
        {
            var shareType = SharePostUnitOfWork.ShareTypes.SingleOrDefault(m => m.Name == vm.ShareType);
            if (shareType == null)
                return false;

            var gender = SharePostUnitOfWork.Genders.SingleOrDefault(m => m.Name == vm.Gender);
            if (gender == null)
                return false;

            Mapper.SharePostVMToSharePost(vm, sharePost, shareType.ShareTypeId, gender.GenderId, isCreation);

            return true;
        }

        private void UrlDecodeSharePostVM(SharePostVM vm)
        {
            //later placeId will be replaced with placeName 
            //then needs to be url decode?
            vm.Suburb = HttpUtility.UrlDecode(vm.Suburb);
            vm.DateAvailableFrom = HttpUtility.UrlDecode(vm.DateAvailableFrom);
            vm.ShareType = HttpUtility.UrlDecode(vm.ShareType);
            vm.Title = HttpUtility.UrlDecode(vm.Title);
            vm.Description = HttpUtility.UrlDecode(vm.Description);
            vm.Gender = HttpUtility.UrlDecode(vm.Gender);
            vm.PostId = HttpUtility.UrlDecode(vm.PostId);
        }
    }
}
