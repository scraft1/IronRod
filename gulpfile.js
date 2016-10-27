/// <binding Clean='clean' />
"use strict";

var gulp = require("gulp"),
  less = require("gulp-less");

var paths = {
    webroot: "./wwwroot/"
};

// gulp.task("less", function() {
//     return gulp.src('Styles/site.less')
//         .pipe(less().on('error', function(err){}))
//         .pipe(gulp.dest(paths.webroot + '/css'));
// });

gulp.task("watchless", ["less"], function(){
    gulp.watch("Styles/*.less", ["less"]);
});