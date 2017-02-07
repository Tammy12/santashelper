define([], function () {
    var vm = {
        friends: [],
        //so activate runs every time this page is selected? Isn't that inefficient if we do searchs on lots of users? 
        //Better to save results in a data.js file?
        activate: function () {
            var self = this;            
            $.ajax({
                type: "GET",
                url: "/Durandal/GetAllUsers",
                datatype: 'json',
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    debugger;
                    self.friends = [];
                    for (var i = 0; i < data.length; i++) {
                        self.friends.push(data[i]);
                    }
                },
                error: function () {
                    debugger;
                }

            });
        }
    };
    return vm;
});