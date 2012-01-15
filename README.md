NSprockets
==========

This should be .NET version of sprockets. The sprockets is [an asset pipeline used in Ruby 3.1](http://guides.rubyonrails.org/asset_pipeline.html).

How it works
------------
You have a manifest files which is a normal javascript files that includes special comments. 
These comments instruct nsproc tool which files should be included in the output.

Example of application.js

    //= require jquery-1.5.1.js
    //= require modernizr-1.7.js
    //= require_self
    
    $(document).ready(function () {
        $("body").append("<h2>It is working</h2><div>This text was appended from application.js script.</div>");
    })

On your web page call `GetScriptFileDeclaration` method in the head part. It will generate appropriate script tags.
    
    <html>
      <head>
        <title>NSprockets Test</title>
        @NSprockets.NSprocketsTool.Current.GetScriptFileDeclaration("application.js")
      </head>
    ...
    </html>

In your web project change post-build event (Project->Properties->Build events to something following.

    $(TargetDir)\nsproc.exe $(TargetDir)\..\scripts\application.js $(TargetDir)\..\scripts\ -minify

It will generate application_<hash>.js file to the scripts directory. The hash is calculated from the content of
all the files included in the main application.js file.

Configuration
-------------
Default configuration values are

* ConcatToSingleFile: true
* Minify: true
* LookupDirectories: true
* OutputDirectory: true

There are two ways how you can change the default configuration.

### web.config

    <add key="NSprockets.ConcatToSingleFile" value="1"/>
    <add key="NSprockets.Minify" value="1"/>
    <add key="NSprockets.LookupDirectories" value="~/scripts,~/scripts2"/>
    <add key="NSprockets.OutputDirectory" value="~/scripts"/>

### Set up the configuration values in Global.asax Application_Start method

        protected void Application_Start()
        {
            NSprocketsTool.Current.ConcatToSingleFile = true;
            NSprocketsTool.Current.Minify = false;
            NSprocketsTool.Current.SetWebOutputDirectory("~/scripts");
            NSprocketsTool.Current.AddServerLookupDirectory("~/scripts");
            NSprocketsTool.Current.AddServerLookupDirectory("~/scripts2");
        }

Already Implemented
-------------------

**Only javascript files are support now!**
Directives in manifest files: `require`, `require_directory`, `require_tree`, `require_self`.

Prototype for coffee preprocessor (it should be probably changed in the future because it is too slow)

Comments
--------

The solution is developed in Microsoft Visual Web Developer 2010 Express. Unit tests are run as a console application.

License
-------
I would like to have it under the _Apache License 2.0_ or _MIT_ but I need to investigate first licenses of used libraries. 

Waiting
-------

* Small refactoring: hiding Asset class - it should be avialable only for unit tests not for consumers. 
* Find out a solution for css files and its resources (changing a paths or how to do that)
