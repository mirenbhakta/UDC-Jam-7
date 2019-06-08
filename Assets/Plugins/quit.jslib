mergeInto(LibraryManager.library, {
	Quit: function () {
		window.history.back();
	},
  
	SyncFiles : function() {
		FS.syncfs(false,function (err) {
			// handle callback
		});
	}
});