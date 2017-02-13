define(["service/shared", "toastr"], function (shared, toast) {
    var vm = {
        shared: shared,

        firstname: ko.observable(''),
        lastname: ko.observable(''),
        email: ko.observable(''),
        password: ko.observable(''),

        loginUser: function () {
            debugger;
            var self = this;
            $.ajax({
                //using GET because POST causes 404/500 errors for some reason...?
                type: "GET",
                url: "/Users/LoginUser",
                data: { email: self.email(), password: self.password() },
                datatype: 'json',
                //had to use context to reset definition of 'this' in callback for some reason
                context: this,
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    debugger;
                    if (data.success) {
                        this.shared.currentUserId = data.id;
                        window.location.href = "#mylist";
                    }
                    else {
                        toast.error(data.message);
                    }
                },
                error: function () {
                    debugger;
                }
            });
        },

        registerNewUser: function () {
            var self = this;
            debugger;
            $.ajax({
                //using GET because POST causes 404/500 errors for some reason...?
                type: "GET",
                url: "/Users/CreateNewUser",
                data: { firstname: self.firstname(), lastname: self.lastname(), email: self.email(), password: self.password() },
                datatype: 'json',
                contentType: "application/json; charset=utf-8",
                success: function(data){
                    debugger;
                    if (data.success) {
                        self.shared.currentUserId = data.id;
                        $('#registerModal').modal('hide');
                        window.location.href = "#mylist";
                        toast.success(data.message);
                    }
                    else {
                        toast.error(data.message);
                    }
                    
                },
                error: function(data){
                    debugger;
                }
               
            });
        }
    };


    return vm;
});