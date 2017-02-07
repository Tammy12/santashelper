define([], function () {
    var vm = {
        firstname: ko.observable(''),
        lastname: ko.observable(''),
        email: ko.observable(''),
        password: ko.observable(''),

        registerNewUser: function () {
            var self = this;
            debugger;
            $.ajax({
                //using GET because POST causes 404/500 errors for some reason...?
                type: "GET",
                url: "/Durandal/CreateNewUser",
                data: { firstname: self.firstname(), lastname: self.lastname(), email: self.email(), password: self.password() },
                datatype: 'json',
                contentType: "application/json; charset=utf-8",
                success: function(){
                    debugger;
                    window.location.href = "#mylist";
                },
                error: function(){
                    debugger;
                }
               
            });
        }
    };


    return vm;
});