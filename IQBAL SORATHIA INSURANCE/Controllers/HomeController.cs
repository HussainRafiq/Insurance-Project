using IQBAL_SORATHIA_INSURANCE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IQBAL_SORATHIA_INSURANCE.Controllers
{
    public class HomeController : Controller
    {
       adinsuranceEntities1 db = new adinsuranceEntities1();
       public static int on = 0;
       public ActionResult login()
       {
           return View();
       }
        [HttpPost]
       public ActionResult login(login l)
       {
            try
            {
                var abc = db.logins.Where(m => m.name.Equals(l.name) && m.password.Equals(l.password)).ToList().Count();
                if (abc > 0)
                {
                    Session["login"] = 1;
                    return RedirectToAction("index");
                }
                ViewBag.incorrect = "Your Username or Password is incorrect";
            }catch(Exception ex)
            {
                ViewBag.incorrect = "Your Username or Password is incorrect "+ex.Message;

            }
            return View();
       }

        public ActionResult Index()
        {
            if (Session["login"]==null)
            {
                return RedirectToAction("login");
            }




            ViewBag.mc = db.marinecovers.Count();
            ViewBag.p = db.policies.Count();
            ViewBag.en = db.endousments.Count();
            ViewBag.re = db.receipts.Count();
            
            
            return View();
        }

        public ActionResult addmarinecovernote()
        {
            if (Session["login"] == null)
            {
                return RedirectToAction("login");
            }

            if (on == 1)
            {
                ViewBag.msg = "Marine Cover Note Registered";
                on = 0;


            }
            return View();
        }
        [HttpPost]

        public ActionResult addmarinecovernote(marinecover m)
        {
            db.marinecovers.Add(m);
            db.SaveChanges();
            on = 1;




            return RedirectToAction("addmarinecovernote");
        }

        public ActionResult showmarinecovernote()
        {
            if (Session["login"] == null)
            {
                return RedirectToAction("login");
            }

            var mcn = db.marinecovers.ToArray().Reverse().ToList();
            var pol = db.policies.ToList();
            var query= from m in mcn
                 join p in pol on m.cno equals p.cno into ou
                 from p in ou.DefaultIfEmpty()
                 select new covernotewithpolicy{ 
                 pol=p,
                 man=m
     };




            return View(query);
        }

        public ActionResult editmarinecovernote(int? id)
        {
            if (Session["login"] == null)
            {
                return RedirectToAction("login");
            }

            var mcn = db.marinecovers.Find(id);


            return View(mcn);
        }
        [HttpPost]
        public ActionResult editmarinecovernote(marinecover man)
        {
            var mcn = db.marinecovers.Find(man.id);
            mcn.cno = man.cno;
            mcn.date = man.date;
            mcn.foreignvalues = man.foreignvalues;
            mcn.item = man.item;
            mcn.nameofbank = man.nameofbank;
            mcn.nameofinsured = man.nameofinsured;
            mcn.pakrs = man.pakrs;
            mcn.equivalentvalue = man.equivalentvalue;
            mcn.remarks = man.remarks;
            mcn.slipmentfrom = man.slipmentfrom;
            db.SaveChanges();



            return RedirectToAction("showmarinecovernote");
        }
        public ActionResult deletemarinecovernote(int id) {
            if (Session["login"] == null)
            {
                return RedirectToAction("login");
            }

            marinecover m = db.marinecovers.Find(id);
            db.marinecovers.Remove(m);
            db.SaveChanges();

            return RedirectToAction("showmarinecovernote");
        }
        public ActionResult detailmarinecovernote(int id)
        {
            if (Session["login"] == null)
            {
                return RedirectToAction("login");
            }
            covernotewithpolicy cwp = new covernotewithpolicy();
marinecover m = db.marinecovers.Find(id);
cwp.man = m;
cwp.pol = db.policies.Where(p => p.cno == m.cno).FirstOrDefault();
            
            return View(cwp);
        }

        public ActionResult addpolicy()
        {
            if (Session["login"] == null)
            {
                return RedirectToAction("login");
            }

            if (on == 1)
            {
                ViewBag.msg = "Policy Registered";
                on = 0;


            }
            ViewBag.type = db.types.ToList();

            int gal = 0, nal = 0;
            var pol = db.policies.ToArray();
            for (int j = 0; j < pol.Length; j++)
            {
                gal += Convert.ToInt32(pol[j].grosspremium);
                nal += Convert.ToInt32(pol[j].netpremium);
            }
            ViewBag.gross = gal;
            ViewBag.net = nal;
                
            return View();
        }
        [HttpPost]
        public ActionResult addpolicy(policy p)
        {
            ViewBag.type = db.types.ToList();
            db.policies.Add(p);
            db.SaveChanges();
            on = 1;
            return RedirectToAction("addpolicy");
        }
        
    [HttpGet]
        public ActionResult showpolicy(int? id)
        {
            if (Session["login"] == null)
            {
                return RedirectToAction("login");
            }

            ViewBag.type = db.types.ToList();
            
        if (id != null)
            {

                var po = db.policies.Where(m => m.type == id).ToArray().Reverse().ToList();
                var to = db.types.ToList();
                var ro = db.receipts.ToList();
                    
            var query = from p in po  
                                     join t in to on p.type equals t.id into table1  
                                     from t in table1.ToList()  
                                     join r in ro on p.policy1 equals r.policy into table2  
                                     from r in table2.ToList() 
                        select new policywithtype
                            {
                                pol = p,
                                typ = t,
                                rep=r
                            };

                return View(query);
            }
        else
        {

            var po = db.policies.ToArray().Reverse().ToList();
            var to = db.types.ToList();
            var ro = db.receipts.ToList();

            var query = from p in po
                        join r in ro on p.policy1 equals r.policy into table2
                        from r in table2.ToList().DefaultIfEmpty()
                        join t in to on p.type equals t.id into table1
                        from t in table1.ToList()
                        select new policywithtype
                        {
                            pol = p,
                            typ = t,
                            rep = r
                        };


            return View(query);
        }
           
        }

        public ActionResult deletepolicy(int id)
        {
            if (Session["login"] == null)
            {
                return RedirectToAction("login");
            }

            policy p = db.policies.Find(id);
            db.policies.Remove(p);
            db.SaveChanges();

            return RedirectToAction("showpolicy");
        }

        public ActionResult detailpolicy(int id)
        {
            if (Session["login"] == null)
            {
                return RedirectToAction("login");
            }


            policywithtype pwt = new policywithtype();
            policy p=db.policies.Find(id);
            pwt.pol = p;
            pwt.typ = db.types.Where(m => m.id == p.type).First();
            var a = db.receipts.Where(m => m.policy == p.policy1);
            if (a.Count() > 0)
            {
               pwt.rep = a.First() ;
            }
            else
            {
                pwt.rep=null;
            }
            return View(pwt);
        }



        public ActionResult Editpolicy(int id)
        {
            if (Session["login"] == null)
            {
                return RedirectToAction("login");
            }

            policy p = db.policies.Find(id);
            ViewBag.type = db.types.ToList();



            return View(p);
        }
        [HttpPost]
        public ActionResult Editpolicy(policy p)
        {
            policy po = db.policies.Find(p.id);
            po.date =p.date;
            po.policy1 = p.policy1;
            po.nameofinsured = p.nameofinsured;
            po.suminsuredvalue = p.suminsuredvalue;
            po.grosspremium = p.grosspremium;
            po.netpremium = p.netpremium;
            po.receipt = p.receipt;
            po.type = p.type;
            po.cno = p.cno;
            po.nameofvessel = p.nameofvessel;
            po.risk = p.risk;
            po.vehicledetail = p.vehicledetail;
            po.detail = p.detail;
            po.@class = p.@class;
            po.endousement = p.endousement;
            po.remarks = p.remarks;
            po.dateofexpiry = p.dateofexpiry;
            db.SaveChanges();



            return RedirectToAction("showpolicy");
        }
        public ActionResult addendousement()
        {
            if (Session["login"] == null)
            {
                return RedirectToAction("login");
            }

            if (on == 1)
            {
                ViewBag.msg = "Endousement Registered";
                on = 0;


            }
         


            return View();
        }
    [HttpPost]
        public ActionResult addendousement(endousment end)
        {

            db.endousments.Add(end);
            db.SaveChanges();
            on = 1;
            return RedirectToAction("addendousement");
        }
    public ActionResult showendousement(string number)
    {
        if (Session["login"] == null)
        {
            return RedirectToAction("login");
        }

        if (number != null) {

            var list = db.endousments.Where(m=>m.endousment1.Equals(number)).ToArray().Reverse().ToList();

  return View(list);
   
        }
        else
        {

            var list = db.endousments.ToArray().Reverse().ToList();
            return View(list);
   
        }
        }

    public ActionResult editendousement(int? id)
    {
        if (Session["login"] == null)
        {
            return RedirectToAction("login");
        }

        var end = db.endousments.Find(id);
        return View(end);
    }
   [HttpPost]
          public ActionResult editendousement(endousment en)
    {
        var end = db.endousments.Find(en.id);
        end.addgross = en.addgross;
        end.addnet = en.addnet;
        end.date = en.date;
        end.dateofexpiry = en.dateofexpiry;
        end.detail = en.detail;
        end.endousment1 = en.endousment1;
        end.lessgross = en.lessgross;
        end.lessnet = en.lessnet;
        end.nameofinsured = en.nameofinsured;
        end.policy = en.policy;
        end.receipt = en.receipt;
        end.remarks = en.remarks;
        end.suminsured = en.suminsured;
        db.SaveChanges();





        return RedirectToAction("showendousement");
    }


        public ActionResult deleteendousement(int? id)
  
    {
        if (Session["login"] == null)
        {
            return RedirectToAction("login");
        }

        endousment en = db.endousments.Find(id);
        db.endousments.Remove(en);
        db.SaveChanges();

        return RedirectToAction("showendousement");
    }
    public ActionResult detailendousement(int? id)
    {
        if (Session["login"] == null)
        {
            return RedirectToAction("login");
        }

        endousment en = db.endousments.Find(id);
        
        return View(en);
    }

    public ActionResult addreceipt()
    {
        if (Session["login"] == null)
        {
            return RedirectToAction("login");
        }

        if (on == 1)
        {
            ViewBag.msg = "Receipt Registered";
            on = 0;


        }



        return View();
    }
    [HttpPost]
    public ActionResult addreceipt(receipt r)
    {
        if (Session["login"] == null)
        {
            return RedirectToAction("login");
        }


        db.receipts.Add(r);
        db.SaveChanges();
        on = 1;
        return RedirectToAction("addreceipt");
    }

    public ActionResult showreceipt()
    {
        if (Session["login"] == null)
        {
            return RedirectToAction("login");
        }

        var list = db.receipts.ToArray().Reverse().ToList();


        return View(list);
    }
    public ActionResult deletereceipt(int? id)
    {
        if (Session["login"] == null)
        {
            return RedirectToAction("login");
        }

        receipt en = db.receipts.Find(id);
        db.receipts.Remove(en);
        db.SaveChanges();

        return RedirectToAction("showreceipt");
    }

    public ActionResult detailreceipt(int? id)
    {
        if (Session["login"] == null)
        {
            return RedirectToAction("login");
        }

        receipt en = db.receipts.Find(id);

        return View(en);
    }

    public ActionResult editreceipt(int? id)
    {
        if (Session["login"] == null)
        {
            return RedirectToAction("login");
        }

        receipt en = db.receipts.Find(id);

        return View(en);
    }
        [HttpPost]
    public ActionResult editreceipt(receipt r)
    {
        if (Session["login"] == null)
        {
            return RedirectToAction("login");
        }

        receipt en = db.receipts.Find(r.id);
        en.date = r.date;
        en.engineering = r.engineering;
        en.fire = r.fire;
        en.health = r.health;
        en.hull = r.hull;
        en.marine = r.marine;
        en.miscilinious = r.miscilinious;
        en.motor = r.motor;
        en.nameofinsured = r.nameofinsured;
        en.policy = r.policy;
        en.receipt1 = r.receipt1;
        en.remainingnet = r.remainingnet;

        db.SaveChanges();

        return RedirectToAction("showreceipt");
    }
  
        
        public ActionResult logout()
    {
        Session.RemoveAll();

        return RedirectToAction("login");
    }

    }
}