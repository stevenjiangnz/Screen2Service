using Screen2.Api.Infrastructure;
using Screen2.Api.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using Microsoft.AspNet.Identity.Owin;
using Screen2.DAL.Interface;
using Screen2.DAL;
using System.Threading.Tasks;

namespace Screen2.Api.Controllers
{
    public class BaseApiController : ApiController
    {
        protected static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class.
        /// </summary>
        /// <param name="unit">The unit.</param>
        public BaseApiController(IUnitWork unit)
        {
            _unit = unit;
        }

        #endregion

        #region Properties
        protected readonly IUnitWork _unit;
        private ModelFactory _modelFactory;
        private ApplicationUserManager _AppUserManager = null;
        private ApplicationRoleManager _AppRoleManager = null;
        #endregion

        #region Methods
        protected ApplicationUserManager AppUserManager
        {
            get
            {
                return _AppUserManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        protected ApplicationRoleManager AppRoleManager
        {
            get
            {
                return _AppRoleManager ?? Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }


        protected ModelFactory TheModelFactory
        {
            get
            {
                if (_modelFactory == null)
                {
                    _modelFactory = new ModelFactory(this.Request, this.AppUserManager);
                }
                return _modelFactory;
            }
        }

        protected IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }


        protected async Task<UserReturnModel> GetCurrentUser()
        {
            UserReturnModel currentUser = null;

            if(User != null)
            {
                var user = await this.AppUserManager.FindByNameAsync(User.Identity.Name);

                if (user != null)
                {
                    currentUser = this.TheModelFactory.Create(user);
                }
            }

            return currentUser;
        }
        #endregion
    }
}