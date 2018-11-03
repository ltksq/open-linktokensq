
using DAL;
using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using LinkTokenSQ.Models;

namespace LinkTokenSQ.Controllers
{
    public class VoteController : BaseController
    {
        public ActionResult Index()
        {
            if (base.GetUserid() != "")
            {
                if (!Common.HeistMon.HMon.GetInstance().AddMon("voteindex_" + base.GetUserid(), 30))
                {
                    this.HttpContext.Response.Write("FireWall!");
                    this.HttpContext.Response.End();
                    return null;
                }
            }
            else
            {
                if (!Common.HeistMon.HMon.GetInstance().AddMon("voteindex_" + base.GetRequesterIP(), 30))
                {
                    this.HttpContext.Response.Write("FireWall!");
                    this.HttpContext.Response.End();
                    return null;
                }
            }

            VoteIndexModel model = new VoteIndexModel();
            model.uid = 0;
            var u = GetUser();
            if (u != null)
            {
                model.uid = u.uid;
            }
            model.votecount=c_voteDal.GetCountByWhere("and votestatus in (2,4)");
            return View(model);
        }


        public ActionResult myvote()
        {
            if (!Common.HeistMon.HMon.GetInstance().AddMon("voteindex_" + base.GetUserid(), 30))
            {
                this.HttpContext.Response.Write("FireWall!");
                this.HttpContext.Response.End();
                return null;
            }

            VoteIndexModel model = new VoteIndexModel();
            model.uid = 0;
            var u = GetUser();
            if (u != null)
            {
                if (!u.wkccheckpass)
                {
                    this.Request.RequestContext.HttpContext.Response.Redirect("/verify", true);
                }
                model.uid = u.uid;
                model.votecount = c_voteDal.GetCountByWhere("and votestatus !=5 and uid="+ model.uid);
            }
           
            return View(model);
        }

        public ActionResult votedetail()
        {
            if (base.GetUserid() != "")
            {
                if (!Common.HeistMon.HMon.GetInstance().AddMon("voteindex_" + base.GetUserid(), 30))
                {
                    this.HttpContext.Response.Write("FireWall!");
                    this.HttpContext.Response.End();
                    return null;
                }
            }
            else
            {
                if (!Common.HeistMon.HMon.GetInstance().AddMon("voteindex_" + base.GetRequesterIP(), 30))
                {
                    this.HttpContext.Response.Write("FireWall!");
                    this.HttpContext.Response.End();
                    return null;
                }
            }
            VoteIndexModel model = new VoteIndexModel();
            model.uid = 0;


            string where = "";
            long vid = 0;
            long svid = 0;
            if (!string.IsNullOrEmpty(Request["vid"])) long.TryParse(Request["vid"], out vid);
            if (!string.IsNullOrEmpty(Request["svid"])) long.TryParse(Request["svid"], out svid);
            where += " and voteid=" + vid;
            if (svid > 0) where += " and svoteid=" + svid;
            if (!string.IsNullOrEmpty(Request["address"]))
            {
                where += string.Format(" and c_voteuser.wkcaddress='{0}' ", Request["address"].Trim());
            }
             
            model.votecount = c_voteuserDal.GetCountByWhere(where);


            return View(model);
        }
        public ActionResult createvote()
        {
            if (!Common.HeistMon.HMon.GetInstance().AddMon("createvote_" + base.GetUserid(), 20))
            {
                this.HttpContext.Response.Write("FireWall!");
                this.HttpContext.Response.End();
                return null;
            }
            var u = GetUser();
            if (!u.wkccheckpass)
            {
                this.Request.RequestContext.HttpContext.Response.Redirect("/verify", true);
            }
           
            VoteIndexModel model = new VoteIndexModel();

            if (LinkTokenSQ.MvcApplication.Syspar.ContainsKey("addvoteprice"))
            {
                double p1 = 0;
                double.TryParse(LinkTokenSQ.MvcApplication.Syspar["addvoteprice"].keyvalue, out p1);
                model.addvoteprice = p1;
            }
            if (LinkTokenSQ.MvcApplication.Syspar.ContainsKey("joinvoteprice"))
            {
                double p1 = 0;
                double.TryParse(LinkTokenSQ.MvcApplication.Syspar["joinvoteprice"].keyvalue, out p1);
                model.joinvoteprice = p1;
            }

            int vid = 0;
            if (!string.IsNullOrEmpty(Request["vid"]))
            {
                vid = int.Parse(Request["vid"]);
            }
            string info = "";
            model.VoteList = new List<c_voteEntity>();
            model.VoteItemList = new List<c_voteitemEntity>();
            if (vid > 0)
            {
                try
                {
                    model.VoteList = c_voteDal.GetListByWhere(" and uid=" + u.uid + " and voteid=" + vid, ref info);
                    if (model.VoteList.Count == 1)
                    {
                        model.VoteItemList = c_voteitemDal.GetListByWhere(" and voteid=" + vid, ref info);
                    }
                }
                catch { }
            }
            return View(model);
        }

        public ActionResult viewvote()
        {
            if (base.GetUserid() != "")
            {
                if (!Common.HeistMon.HMon.GetInstance().AddMon("voteindex_" + base.GetUserid(), 30))
                {
                    this.HttpContext.Response.Write("FireWall!");
                    this.HttpContext.Response.End();
                    return null;
                }
            }
            else
            {
                if (!Common.HeistMon.HMon.GetInstance().AddMon("voteindex_" + base.GetRequesterIP(), 30))
                {
                    this.HttpContext.Response.Write("FireWall!");
                    this.HttpContext.Response.End();
                    return null;
                }
            }

            VoteIndexModel model = new VoteIndexModel();

            if (LinkTokenSQ.MvcApplication.Syspar.ContainsKey("addvoteprice"))
            {
                double p1 = 0;
                double.TryParse(LinkTokenSQ.MvcApplication.Syspar["addvoteprice"].keyvalue, out p1);
                model.addvoteprice = p1;
            }
            if (LinkTokenSQ.MvcApplication.Syspar.ContainsKey("joinvoteprice"))
            {
                double p1 = 0;
                double.TryParse(LinkTokenSQ.MvcApplication.Syspar["joinvoteprice"].keyvalue, out p1);
                model.joinvoteprice = p1;
            }

            int vid = 0;
            if (!string.IsNullOrEmpty(Request["vid"]))
            {
                vid = int.Parse(Request["vid"]);
            }
            string info = "";
            model.VoteList = new List<c_voteEntity>();
            model.VoteItemList = new List<c_voteitemEntity>();
            if (vid > 0)
            {
                try
                {
                    model.VoteList = c_voteDal.GetListByWhere(" and voteid=" + vid, ref info);
                    if (model.VoteList.Count == 1)
                    {
                        model.VoteItemList = c_voteitemDal.GetListByWhere(" and voteid=" + vid, ref info);
                        model.VoteJoinUserList = c_voteuserDal.GetListByWhere(" and voteid=" + vid, ref info);
                    }
                }
                catch { }
            }
            var uinfo = this.GetUser();
            if (uinfo != null && uinfo.uid > 0 && uinfo.wkccheckpass) model.uid = uinfo.uid;
            return View(model);

        }
        [HttpPost]
        public JsonResult submitvote(string title, string remark, DateTime enddte, int vid = 0)
        {

            if (!Common.HeistMon.HMon.GetInstance().AddMon("submitvote_" + base.GetUserid(), 20))
            {
                this.HttpContext.Response.Write("FireWall!");
                this.HttpContext.Response.End();
                return null;
            }

            AjaxResult ajax = new AjaxResult();
            ajax.ErrorMessage = "";
            ajax.RetCode = 1;


            var u = GetUser();
            if (u == null || u.uid <= 0)
            {
                ajax.ErrorMessage = "用户登录超时,请重新登录.";
                return Json(ajax);
            }
            if (u.isvip == 0 && u.vipdate <= DateTime.Now.Date)
            {
                ajax.ErrorMessage = "您无发起投票的权限,请联系社区相关人员.";
                return Json(ajax);
            }

            if (string.IsNullOrEmpty(title.Trim()))
            {
                ajax.ErrorMessage = "投票主题未填.";
                return Json(ajax);
            }
            if (enddte < DateTime.Now.Date.AddDays(1))
            {
                ajax.ErrorMessage = "日期不能小于当天.";
                return Json(ajax);
            }
            double addvoteprice = 0;
            double joinvoteprice = 0;
            if (vid == 0)
            {
                if (LinkTokenSQ.MvcApplication.Syspar.ContainsKey("addvoteprice"))
                {
                    double.TryParse(LinkTokenSQ.MvcApplication.Syspar["addvoteprice"].keyvalue, out addvoteprice);

                }
                if (LinkTokenSQ.MvcApplication.Syspar.ContainsKey("joinvoteprice"))
                {
                    double.TryParse(LinkTokenSQ.MvcApplication.Syspar["joinvoteprice"].keyvalue, out joinvoteprice);
                }
            }
            string info = "";
            c_voteEntity vote = new c_voteEntity();
            if (vid > 0)
            {
                var list = c_voteDal.GetListByWhere(string.Format(" and voteid={0} and uid={1} ", vid, u.uid), ref info);
                if (list == null || list.Count == 0)
                {
                    ajax.ErrorMessage = "数据未找到.";
                    return Json(ajax);
                }
                vote = list[0];
            }
            else
            {
                vote.createtime = DateTime.Now;
            }
            vote.lasttime = DateTime.Now;
            vote.votetitle = title;
            vote.votestatus = 0;
            vote.votetype = 0;
            vote.remark = remark;
            vote.paywkcamount = addvoteprice;
            vote.uid = u.uid;
            vote.voteid = vid;
            vote.enddate = enddte;
            if (vote.voteid == 0)
            {
                long voteid = c_voteDal.Insert(vote);

                if (voteid > 0)
                {
                    if (addvoteprice > 0)
                    {
                        if (DBAccess.UserPay(u.uid, addvoteprice) != 1)
                        {
                            c_voteDal.Del(voteid.ToString(), ref info);
                            ajax.ErrorMessage = "支付失败,可能账户余额不足.";
                            return Json(ajax);
                        }
                    }
                    ajax.ErrorMessage = voteid.ToString();
                }
                else
                {
                    ajax.ErrorMessage = "保存失败,请重试试.";
                    return Json(ajax);
                }
            }
            else
            {
                if (vote.votestatus > 3)
                {
                    ajax.ErrorMessage = "不能修改.";
                    return Json(ajax);
                }
                if (!c_voteDal.Update(vote, ref info))
                {
                    ajax.ErrorMessage = "保存失败,请重试试.";
                    return Json(ajax);
                }
                else
                {
                    ajax.ErrorMessage = "已保存.";
                    return Json(ajax);
                }
            }
            ajax.RetCode = 0;
            return Json(ajax);
        }

        [HttpPost]
        public JsonResult submitvoteitem(string title, int vid)
        {
            if (!Common.HeistMon.HMon.GetInstance().AddMon("submitvote_" + base.GetUserid(), 20))
            {
                this.HttpContext.Response.Write("FireWall!");
                this.HttpContext.Response.End();
                return null;
            }


            AjaxResult ajax = new AjaxResult();
            ajax.ErrorMessage = "";
            ajax.RetCode = 1;

            if (string.IsNullOrEmpty(title.Trim()))
            {
                ajax.ErrorMessage = "投票项目名未填.";
                return Json(ajax);
            }


            var u = GetUser();
            if (u == null || u.uid <= 0)
            {
                ajax.ErrorMessage = "用户登录超时,请重新登录.";
                return Json(ajax);
            }


            string info = "";
            var list = c_voteDal.GetListByWhere(" and uid=" + u.uid + " and voteid=" + vid, ref info);
            if (list == null || list.Count == 0)
            {
                ajax.ErrorMessage = "数据未找到.";
                return Json(ajax);
            }
            if (list[0].enddate <= DateTime.Now.Date)
            {
                ajax.ErrorMessage = "投票已结束,不能操作.";
                return Json(ajax);
            }
            if (list[0].votestatus != 0)
            {
                ajax.ErrorMessage = "投票已结束,不能操作.";
                return Json(ajax);
            }
            var slist = c_voteitemDal.GetListByWhere(" and voteid=" + vid, ref info);
            if (slist.Count > 10)
            {
                ajax.ErrorMessage = "投票项不能超过10项.";
                return Json(ajax);
            }
            c_voteitemEntity vote = new c_voteitemEntity();
            vote.voteid = vid;
            vote.lasttime = DateTime.Now;
            vote.title = title;
            vote.createtime = DateTime.Now;
            if (c_voteitemDal.Insert(vote) > 0)
            {
                ajax.ErrorMessage = vid.ToString();
            }
            else
            {
                ajax.ErrorMessage = "保存失败,请重试试.";
                return Json(ajax);
            }
            ajax.RetCode = 0;
            return Json(ajax);
        }

        [HttpPost]
        public JsonResult delvoteitem(int vid, int ivid)
        {
            if (!Common.HeistMon.HMon.GetInstance().AddMon("submitvote_" + base.GetUserid(), 20))
            {
                this.HttpContext.Response.Write("FireWall!");
                this.HttpContext.Response.End();
                return null;
            }


            AjaxResult ajax = new AjaxResult();
            ajax.ErrorMessage = "";
            ajax.RetCode = 1;


            var u = GetUser();
            if (u == null || u.uid <= 0)
            {
                ajax.ErrorMessage = "用户登录超时,请重新登录.";
                return Json(ajax);
            }


            string info = "";
            var list = c_voteDal.GetListByWhere(" and uid=" + u.uid + " and voteid=" + vid, ref info);
            if (list == null || list.Count == 0)
            {
                ajax.ErrorMessage = "数据未找到.";
                return Json(ajax);
            }
            if (list[0].enddate <= DateTime.Now.Date)
            {
                ajax.ErrorMessage = "投票已结束,不能操作.";
                return Json(ajax);
            }
            if (list[0].votestatus != 0)
            {
                ajax.ErrorMessage = "已进入投票或已结束,不能操作.";
                return Json(ajax);
            }
            var slist = c_voteitemDal.GetByKeyId(ivid, ref info);
            if (slist == null || slist.svoteid <= 0)
            {
                ajax.ErrorMessage = "数据未找到.";
                return Json(ajax);
            }

            if (c_voteitemDal.Del(ivid.ToString(), ref info))
            {
                ajax.ErrorMessage = vid.ToString();
            }
            else
            {
                ajax.ErrorMessage = "操作失败,请重试试.";
                return Json(ajax);
            }
            ajax.RetCode = 0;
            return Json(ajax);
        }

        [HttpPost]
        public JsonResult pushvote(int vid)
        {
            if (!Common.HeistMon.HMon.GetInstance().AddMon("submitvote_" + base.GetUserid(), 20))
            {
                this.HttpContext.Response.Write("FireWall!");
                this.HttpContext.Response.End();
                return null;
            }


            AjaxResult ajax = new AjaxResult();
            ajax.ErrorMessage = "";
            ajax.RetCode = 1;


            var u = GetUser();
            if (u == null || u.uid <= 0)
            {
                ajax.ErrorMessage = "用户登录超时,请重新登录.";
                return Json(ajax);
            }


            string info = "";
            var list = c_voteDal.GetListByWhere(" and uid=" + u.uid + " and voteid=" + vid, ref info);
            if (list == null || list.Count == 0)
            {
                ajax.ErrorMessage = "数据未找到.";
                return Json(ajax);
            }
             
            if (list[0].votestatus != 0)
            {
                ajax.ErrorMessage = "已进入投票或已结束,不能操作.";
                return Json(ajax);
            }
            list[0].votestatus = 2;
            if (c_voteDal.Update(list[0], ref info))
            {
                ajax.ErrorMessage = vid.ToString();
            }
            else
            {
                ajax.ErrorMessage = "操作失败,请重试试.";
                return Json(ajax);
            }
            ajax.RetCode = 0;
            return Json(ajax);
        }


        [HttpPost]
        public JsonResult endvote(int vid)
        {
            if (!Common.HeistMon.HMon.GetInstance().AddMon("submitvote_" + base.GetUserid(), 20))
            {
                this.HttpContext.Response.Write("FireWall!");
                this.HttpContext.Response.End();
                return null;
            }


            AjaxResult ajax = new AjaxResult();
            ajax.ErrorMessage = "";
            ajax.RetCode = 1;


            var u = GetUser();
            if (u == null || u.uid <= 0)
            {
                ajax.ErrorMessage = "用户登录超时,请重新登录.";
                return Json(ajax);
            }


            string info = "";
            var list = c_voteDal.GetListByWhere(" and uid=" + u.uid + " and voteid=" + vid, ref info);
            if (list == null || list.Count == 0)
            {
                ajax.ErrorMessage = "数据未找到.";
                return Json(ajax);
            }

            if (list[0].votestatus != 2)
            {
                ajax.ErrorMessage = "当前状态不能操作.";
                return Json(ajax);
            }
            list[0].votestatus = 4;
            if (c_voteDal.Update(list[0], ref info))
            {
                ajax.ErrorMessage = vid.ToString();
            }
            else
            {
                ajax.ErrorMessage = "操作失败,请重试试.";
                return Json(ajax);
            }
            ajax.RetCode = 0;
            return Json(ajax);
        }

        [HttpPost]
        public JsonResult ajaxgetvotelist(string key, string status, int pagenum = 1)
        {
            AjaxResult ajax = new AjaxResult();
            ajax.ErrorMessage = "";
            ajax.RetCode = 0;

            VoteIndexModel model = new VoteIndexModel();
            model.myvote = key == "my";
            string where = "";
            if (model.myvote)
            {
                var u = GetUser();
                where = " and uid=" + (u != null ? u.uid : 0) + " and votestatus !=5 order by votestatus ,createtime desc ";
            }
            else
            {
                where = " and votestatus in (2,4) order by votestatus ,createtime desc ";
            }
           
            model.VoteList=c_voteDal.InfoPagerData(where, 10, pagenum);
            int count = c_voteDal.GetCountByWhere(where);
            string html = base.RenderViewToString("votelist", model);
           
            ajax.Html = html;
            
            ajax.ErrorMessage = (count / 10 + 2).ToString();
            ajax.RetCode = count > 0 ? 1 : 0;

            return Json(ajax);
        }


        [HttpPost]
        public JsonResult joinvoteitem(int vid, int ivid)
        {
            if (!Common.HeistMon.HMon.GetInstance().AddMon("joinvoteitem_" + base.GetUserid(), 20))
            {
                this.HttpContext.Response.Write("FireWall!");
                this.HttpContext.Response.End();
                return null;
            }


            AjaxResult ajax = new AjaxResult();
            ajax.ErrorMessage = "";
            ajax.RetCode = 1;


            var u = GetUser();
            if (u == null || u.uid <= 0)
            {
                ajax.ErrorMessage = "用户未登录.";
                return Json(ajax);
            }
            if (!u.wkccheckpass)
            {
                ajax.ErrorMessage = "请先验证钱包有效性.";
                return Json(ajax);
            }

            string info = "";
            var list = c_voteDal.GetListByWhere(" and voteid=" + vid, ref info);
            if (list == null || list.Count == 0)
            {
                ajax.ErrorMessage = "数据未找到.";
                return Json(ajax);
            }
            if (list[0].enddate <= DateTime.Now.Date)
            {
                ajax.ErrorMessage = "投票已结束,不能操作.";
                return Json(ajax);
            }
            if (list[0].votestatus != 2)
            {
                ajax.ErrorMessage = "投票已结束,不能操作.";
                return Json(ajax);
            }
            var slist = c_voteitemDal.GetByKeyId(ivid, ref info);
            if (slist == null || slist.svoteid <= 0)
            {
                ajax.ErrorMessage = "投票项未找到.";
                return Json(ajax);
            }
            lock (u)
            {

                int useritem = c_voteuserDal.GetCountByWhere(" and voteid="+vid+" and uid="+u.uid);
                if (list[0].votetype == 1)
                {
                    useritem = c_voteuserDal.GetCountByWhere(" and svoteid=" + ivid + " and uid=" + u.uid);
                }
                if (useritem > 0)
                {
                    ajax.ErrorMessage = "您已经投过票.";
                    return Json(ajax);
                }
                else
                {
                    var userinfo = uinfoDal.GetByKeyId(u.uid, ref info);
                    if (userinfo == null)
                    {
                        ajax.ErrorMessage = "用户信息有未找到.";
                        return Json(ajax);
                    }
                    c_voteuserEntity obj = new c_voteuserEntity();
                    obj.uid = u.uid;
                    obj.svoteid = ivid;
                    obj.voteid = vid;
                    obj.createtime = DateTime.Now;
                    obj.wkcaddress = userinfo.wkcaddress;
                    obj.wkcamount = 0;
                    #region 获取wkc余额
                    if (obj.wkcaddress.Trim() == "")
                    {
                        ajax.ErrorMessage = "未绑定钱包地址.";
                        return Json(ajax);

                    }
                    try
                    {
                        WebClient MyWebClient = new WebClient();
                        MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据
                        Byte[] pageData = MyWebClient.DownloadData("http://xxx?action=getamount&address=" + obj.wkcaddress.Trim()); //从指定网站下载数据
                        string pageHtml = Encoding.Default.GetString(pageData);  //如果获取网站页面采用的是GB2312，则使用这句    
                                                                                 //string pageHtml = Encoding.UTF8.GetString(pageData); //如果获取网站页面采用的是UTF-8，则使用这句
                        if (pageHtml.Length>22 && pageHtml.Trim() == "-99")
                        {
                            ajax.ErrorMessage = "未获取到钱包余额.";
                            return Json(ajax);
                        }
                        double amount = 0;
                        double.TryParse(pageHtml, out amount);
                        obj.wkcamount = amount;
                        obj.getamountok = 1;

                        double joinvoteprice = 0;
                        if (LinkTokenSQ.MvcApplication.Syspar.ContainsKey("joinvoteprice"))
                        {
                
                            double.TryParse(LinkTokenSQ.MvcApplication.Syspar["joinvoteprice"].keyvalue, out joinvoteprice);
                          
                        }
                        long id=c_voteuserDal.Insert(obj);
                        if (id > 0)
                        {
                            if (joinvoteprice > 0)
                            {
                                if (DBAccess.UserPay(u.uid, joinvoteprice) != 1)
                                {
                                    c_voteuserDal.Del(id.ToString(), ref info);
                                    ajax.ErrorMessage = "支付失败,可能账户余额不足.";
                                    return Json(ajax);
                                }
                            }

                            ajax.ErrorMessage = vid.ToString();
                          
                        }
                        else
                        {
                            ajax.ErrorMessage = "投票失败，请重试试.";
                            return Json(ajax);
                        }

                    }
                    catch
                    {
                        ajax.ErrorMessage = "未获取到钱包余额.";
                        return Json(ajax);
                    }
                    #endregion
                }

            }
         
           
            ajax.RetCode = 0;
            return Json(ajax);
        }


        [HttpPost]
        public JsonResult ajaxgetvotedetail(string key, string status, int pagenum = 1)
        {
            AjaxResult ajax = new AjaxResult();
            ajax.ErrorMessage = "";
            ajax.RetCode = 0;

            VoteIndexModel model = new VoteIndexModel();
            model.VoteJoinUserList = new List<c_voteuserEntity>();
            string where = "";
            long vid = 0;
            long svid = 0;
            if (!string.IsNullOrEmpty(Request["vid"])) long.TryParse(Request["vid"], out vid);
            if (!string.IsNullOrEmpty(Request["svid"])) long.TryParse(Request["svid"], out svid);
            where += " and voteid=" + vid;
            if (svid > 0) where += " and svoteid=" + svid;
            if (!string.IsNullOrEmpty(Request["address"]))
            {
                where += string.Format(" and c_voteuser.wkcaddress='{0}' ", Request["address"].Trim());
            }
            model.VoteJoinUserList = c_voteuserDal.InfoPagerData(where+ " order by usvoteid desc ", 10, pagenum);
            int count = c_voteuserDal.GetCountByWhere(where);
            string html = base.RenderViewToString("_detail", model);

            ajax.Html = html;

            ajax.ErrorMessage = (count / 10 + 2).ToString();
            ajax.RetCode = count > 0 ? 1 : 0;

            return Json(ajax);
        }
    }
}