define(['plugins/router', 'durandal/app', "service/shared"], function (router, app, shared) {
    return {
        shared: shared,

        router: router,
        search: function() {
            //It's really easy to show a message box.
            //You can add custom options too. Also, it returns a promise for the user's response.
            app.showMessage('Search not yet implemented...');
        },
        activate: function () {
            debugger;
            router.map([
                //{ route: '', title:'Welcome', moduleId: 'viewmodels/welcome', nav: true },
                //{ route: 'flickr', moduleId: 'viewmodels/flickr', nav: true },
                {route: '', title: 'Register', moduleId: 'viewmodels/register', nav: false},
                { route: 'mylist', moduleId: 'viewmodels/mylist', nav: true },
                { route: 'friends', moduleId: 'viewmodels/friends', nav: true },
                { route: 'friendlist/:id', moduleId: 'viewmodels/friendlist', nav: false },
                { route: 'shopping', moduleId: 'viewmodels/shopping', nav: true }
            ]).buildNavigationModel();
            
            return router.activate();
        },
        currentRoute: ko.computed(function () {
            debugger;
            if (router.activeInstruction() != null)
                return router.activeInstruction().fragment;
            else return null;
        }),
        logOut: function () {
            debugger;
            var self = this;
            self.currentUserId = null;
            window.location.href = "";
        }
    };
});