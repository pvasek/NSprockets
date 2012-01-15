NSprockets
==========

This should be .NET version of sprockets. The sprockets is asset pipeline used in Ruby 3.1.

The solution is developed in Microsoft Visual Web Developer 2010 Express. Unit tests are run as a console application.

Already Implemented
-------------------
Directives in manifest files: require, require_directory, require_tree, require_self.
Example of coffee preprocessor (it should be probably changed in the future because it is too slow)

Waiting
-------
* Small refactoring: hiding Asset class - it should be avialable only for unit tests not for consumers. 
* Compressing js files
* Find out a solution for css files and its resources (changing a paths or how to do that)
* At least a simple documentation and a sample project