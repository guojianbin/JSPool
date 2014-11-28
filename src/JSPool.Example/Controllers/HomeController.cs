﻿/*
 * Copyright (c) 2014 Daniel Lo Nigro (Daniel15)
 * 
 * This source code is licensed under the BSD-style license found in the 
 * LICENSE file in the root directory of this source tree. 
 */

using System;
using System.Web.Mvc;
using JSPool.Example.Models;

namespace JSPool.Example.Controllers
{
    public class HomeController : Controller
    {
	    private IJsPool GetJsPool()
	    {
			// In a real app you'd use your preferred dependency injection container here
			return (IJsPool)HttpContext.Application["jspool"];
	    }

        public ActionResult Index()
        {
	        var pool = GetJsPool();
            return View(new HomeViewModel
            {
	            AvailableEngineCount = pool.AvailableEngineCount,
				EngineCount = pool.EngineCount,
            });
        }

	    public ActionResult HelloWorld()
	    {
		    var pool = GetJsPool();
			var engine = pool.GetEngine();
			// This function is created in JsPoolInitializer.cs
		    var result = engine.CallFunction<string>("helloWorld");
			// Always release an engine when you're done with it.
			pool.ReturnEngineToPool(engine);

		    return Content(result);
	    }

	    public ActionResult Loop()
	    {
		    var pool = GetJsPool();
		    var engine = pool.GetEngine();
			engine.Execute(@"
				var timeToEnd = Date.now() + 10000;
				while (Date.now() < timeToEnd) { }
			");
			// Always release an engine when you're done with it.
			pool.ReturnEngineToPool(engine);

		    return Content("Done " + DateTime.Now);
	    }
	}
}