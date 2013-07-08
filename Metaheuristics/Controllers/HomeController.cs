using Metaheuristics.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Metaheuristics.Controllers
{
    public class HomeController : Controller
    {
        private static List<double> _xs = new List<double>();
        private static List<double> _ys = new List<double>();
        private static int _iterations = 0;
        private static int _iterationsPerStep = 10;
        private static int _progress = 0;
        private static Cities _cityList = new Cities();
        private static Tsp _tsp = new Tsp();


        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Init(int it, int perStep, string[] xs, string[] ys)
        {
            _xs = new List<double>();
            _ys = new List<double>();
            _iterations = 0;
            _iterationsPerStep = 10;
            _progress = 0;
            _cityList = new Cities();
            _tsp = new Tsp();

            NumberFormatInfo format = new NumberFormatInfo();
            format.NumberGroupSeparator = ".";

            foreach (var coord in xs)
            {
                _xs.Add(Double.Parse(coord, format));
            }

            foreach (var coord in ys)
            {
                _ys.Add(Double.Parse(coord, format));
            }
            _iterations = it;
            _iterationsPerStep = perStep;

            //Cities cityList = new Cities();

            for (int i = 0; i < _xs.Count; i++)
            {
                City city = new City(Convert.ToInt32(_xs[i]), Convert.ToInt32(_ys[i]));
                _cityList.Add(city);
            }

            _cityList.CalculateCityDistances(5);

            _tsp.Begin(10000, 1, 5, 3, 0, 90, _cityList);

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Iterate()
        {
            _tsp.Iterate(_iterationsPerStep, 5, 3);
            _progress += _iterationsPerStep;

            Tour best = _tsp.population.BestTour;

            StringBuilder sb = new StringBuilder();

            int lastCity = 0;
            int nextCity = best[0].Connection1;

            foreach (City city in _tsp.cityList)
            {
                sb.AppendFormat("M{0} {1}L{2} {3}", _cityList[lastCity].Location.X, _cityList[lastCity].Location.Y,
                    _cityList[nextCity].Location.X, _cityList[nextCity].Location.Y);

                // изчисляване дали следващия град е записан на Connection1 или Connection2
                if (lastCity != best[nextCity].Connection1)
                {
                    lastCity = nextCity;
                    nextCity = best[nextCity].Connection1;
                }
                else
                {
                    lastCity = nextCity;
                    nextCity = best[nextCity].Connection2;
                }
            }

            return Json(new { cont = _progress < _iterations, cmd = sb.ToString() }, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public ActionResult Calculate(int it, List<double> xs, List<double> ys)
        //{
        //    Tsp tsp = new Tsp();
        //    Cities cityList = new Cities();

        //    for (int i = 0; i < xs.Count; i++)
        //    {
        //        City city = new City(Convert.ToInt32(xs[i]), Convert.ToInt32(ys[i]));
        //        cityList.Add(city);
        //    }

        //    cityList.CalculateCityDistances(5);
        //    tsp.Begin(10000, it, 5, 3, 0, 90, cityList);
        //    Tour best = tsp.population.BestTour;

        //    StringBuilder sb = new StringBuilder();

        //    int lastCity = 0;
        //    int nextCity = best[0].Connection1;

        //    foreach (City city in tsp.cityList)
        //    {
        //        sb.AppendFormat("M{0} {1}L{2} {3}", cityList[lastCity].Location.X, cityList[lastCity].Location.Y,
        //            cityList[nextCity].Location.X, cityList[nextCity].Location.Y);

        //        // изчисляване дали следващия град е записан на Connection1 или Connection2
        //        if (lastCity != best[nextCity].Connection1)
        //        {
        //            lastCity = nextCity;
        //            nextCity = best[nextCity].Connection1;
        //        }
        //        else
        //        {
        //            lastCity = nextCity;
        //            nextCity = best[nextCity].Connection2;
        //        }
        //    }

        //    return Json(new { cmd = sb.ToString() }, JsonRequestBehavior.AllowGet);
        //}
    }
}
