define(["service/shared"], function (shared) {
    var vm = {
        shared: shared,

        friends: [],
        newRow: function (friend) {
            debugger;
            this.name = friend.firstname;
            this.id = friend.id;
            this.hover = ko.observable(false);
        },
        showSeeList: function(row) {
            row.hover(true);
        },
        hideSeeList: function (row) {
            row.hover(false);
        },

        //so activate runs every time this page is selected? Isn't that inefficient if we do searchs on lots of users? 
        //Better to save results in a data.js file?
        activate: function () {
            debugger;
            var self = this;            
            $.ajax({
                type: "GET",
                url: "/Users/GetFriendUsers",
                data: {currentUserId: self.shared.currentUserId},
                datatype: 'json',
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    debugger;
                    self.friends = [];
                    for (var i = 0; i < data.length; i++) {
                        var row = new self.newRow(data[i]);
                        self.friends.push(row);
                    }
                },
                error: function () {
                    debugger;
                }

            });
            debugger;
        }
    };
    return vm;
});