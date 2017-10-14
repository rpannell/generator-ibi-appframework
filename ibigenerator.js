
'use strict';

var newFiles = [],
	folders = [],
	fs = require('fs'),
	tfs = require('./tfs-unlock'),
	path = require('path'),
	glob = require('glob'),
	xmldom = require('xmldom').DOMParser,
	xmlserial = require('xmldom').XMLSerializer, 
	helper;
	
const winston = require('winston');  	
winston.configure({
	transports: [new (winston.transports.File)({ filename: path.join(process.env.APPDATA, "ApplicationFrameworkManagement", "logs", "app.log") })]
});
	
tfs.init({
	"visualStudioPath": tfs.vs2017.bit64
});	


helper = { 
	log: function(extraData, object){
		winston.info(extraData, object);
	},
	checkout: function(filePath){
		var paths = [];
		paths.push(filePath);
		tfs.checkout(paths);
	},
	addfile: function(filePath){
		if(newFiles == null || newFiles == undefined){
			newFiles = [];
		}
		newFiles.push(filePath);
		var dirName = path.dirname(filePath);
		folders.push(dirName);
	},
	addfolder: function(folderPath){
		folders.push(folderPath);
	},
	netWebApi: function(sourceLocation, progFile){
		
		console.log("NewFiles Service: ");
		console.log(newFiles);
		fs.readFile(path.join(sourceLocation, progFile), 'utf-8', function(err, data) {
			var doc = new xmldom().parseFromString(data, 'text/xml');
			var items = doc.getElementsByTagName('Project');
			var foundIt = false;
			for(var i in items){
				var item = items[i];
				if(item.documentElement != undefined){
					if(item.documentElement.tagName == "Project"){
						for(var j = 0; j < item.documentElement.childNodes.length; j++){
							var e = item.documentElement.childNodes[j];
							if(e.tagName == "ItemGroup" && e.hasChildNodes()){
								for(var k = 0; k < e.childNodes.length; k++){
									if(e.childNodes[k].tagName == "Compile"){
										foundIt = true;
										break;
									}
								}
								
								if(foundIt){
									for(var f = 0; f < newFiles.length; f++){
										var file = newFiles[f];
										console.log("F: " + f);
										console.log("File: " + file);
										console.log("New File: " + newFiles[f]);
										if(file.includes(sourceLocation)){
											var newCompile = doc.createElement("Compile");
											newCompile.setAttribute("Include", file.replace(sourceLocation + "\\", ""));
											e.appendChild(newCompile);
										}
									}
								
									
									break;
								}
							}
						}
					}
				}
			}
			fs.writeFileSync(path.join(sourceLocation, progFile), new xmlserial().serializeToString(doc));	
		});
	},
	netWebApiFiles: function(sourceLocation, progFile, files){
		console.log("Files Service: ");
		console.log(files);
		fs.readFile(path.join(sourceLocation, progFile), 'utf-8', function(err, data) {
			var doc = new xmldom().parseFromString(data, 'text/xml');
			var items = doc.getElementsByTagName('Project');
			var foundIt = false;
			for(var i in items){
				var item = items[i];
				if(item.documentElement != undefined){
					if(item.documentElement.tagName == "Project"){
						for(var j = 0; j < item.documentElement.childNodes.length; j++){
							var e = item.documentElement.childNodes[j];
							if(e.tagName == "ItemGroup" && e.hasChildNodes()){
								for(var k = 0; k < e.childNodes.length; k++){
									if(e.childNodes[k].tagName == "Compile"){
										foundIt = true;
										break;
									}
								}
								
								if(foundIt){
									for(var f = 0; f < files.length; f++){
										var file = files[f];
										if(file.includes(sourceLocation)){
											var newCompile = doc.createElement("Compile");
											newCompile.setAttribute("Include", file.replace(sourceLocation + "\\", ""));
											e.appendChild(newCompile);
										}
									}
								
									
									break;
								}
							}
						}
					}
				}
			}
			fs.writeFileSync(path.join(sourceLocation, progFile), new xmlserial().serializeToString(doc));	
		});
	},
	standardPluginProj: function(sourceLocation, progFile){
		fs.readFile(path.join(sourceLocation, progFile), 'utf-8', function(err, data) {
			var doc = new xmldom().parseFromString(data, 'text/xml');
			var items = doc.getElementsByTagName('Project');
			var foundIt = false;
			for(var i in items){
				var item = items[i];
				if(item.documentElement != undefined){
					if(item.documentElement.tagName == "Project"){
						for(var j = 0; j < item.documentElement.childNodes.length; j++){
							var e = item.documentElement.childNodes[j];
							if(e.tagName == "ItemGroup" && e.hasChildNodes()){
								for(var k = 0; k < e.childNodes.length; k++){
									if(e.childNodes[k].tagName == "Compile"){
										foundIt = true;
										break;
									}
								}
								
								if(foundIt){
									for(var f = 0; f < newFiles.length; f++){
										var file = newFiles[f];
										if(file.includes(sourceLocation)){
											var newCompile = doc.createElement("Compile");
											newCompile.setAttribute("Include", file.replace(sourceLocation + "\\", ""));
											e.appendChild(newCompile);
										}
									}
								
									
									break;
								}
							}
						}
					}
				}
			}
			fs.writeFileSync(path.join(sourceLocation, progFile), new xmlserial().serializeToString(doc));	
		});
	}
};
exports.log = function (extraData, object) {
	helper.log(extraData, object);
};
exports.resetFilesAndFolders = function(){
	folders = [];
	newFiles = [];
};
exports.addfile = function (filePath) {
	helper.addfile(filePath);
};
exports.addfolder = function (folderPath) {
	helper.addfolder(folderPath);
};
exports.checkout = function (filePath) {
	helper.checkout(filePath);
};

exports.checkoutoradd = function (filePath) {
	if(fs.existsSync(filePath)){
		helper.checkout(filePath);
	} else {
		helper.addfile(filePath);
	}
};

exports.writeScaffoldingToProj = function(sourceLocation, serviceFiles){
	//find the prog file
	glob("*.csproj", { cwd: sourceLocation }, function(er, files){
		var progFile = "";
		if(er == null && files != null && files.length > 0){
			progFile = files[0];
		}
		
		helper.checkout(path.join(sourceLocation, progFile));
		helper.netWebApiFiles(sourceLocation, progFile, serviceFiles);
	});
}

exports.writeScaffoldingToPluginProj = function(sourceLocation){
	//find the prog file
	glob("*.csproj", { cwd: sourceLocation }, function(er, files){
		var progFile = "";
		if(er == null && files != null && files.length > 0){
			progFile = files[0];
		}
		
		console.log("Plugin Loc: " + sourceLocation);
		console.log("Plugin Proj: " + progFile);
		console.log(newFiles);
		
		helper.checkout(path.join(sourceLocation, progFile));
		helper.standardPluginProj(sourceLocation, progFile);
	});
}

exports.addNewFolderToTFS = function(sourceLocation){
	glob("**/*.*", { cwd: sourceLocation }, function(er, files){
		for(var i = 0; i < files.length; i++){
			var fullpath = path.join(sourceLocation, files[i]);
			helper.addfile(fullpath);
		}
		
		tfs.add(folders);
		tfs.add(newFiles);
	});
	
}

exports.doTfsOperations = function(){
	tfs.add(folders);
	tfs.add(newFiles);
};