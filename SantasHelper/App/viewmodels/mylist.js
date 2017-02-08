define(['knockout'], function (ko) {
    var vm = {
        displayName: 'My Wish List',
        newWish: ko.observable(''),
        wishes: ko.observableArray([]),
        newRow: function(title) {
            this.name = title;
            this.hover = ko.observable(false);
        },
        addWish: function () {
            debugger;
            var self = this;

            //update mysql database
            $.ajax({
                type: "GET",
                url: "/MyList/AddNewWish",
                data: { item: self.newWish() },
                datatype: 'json',
                contentType: "application/json; charset=utf-8",
                success: function () {
                    debugger;
                },
                error: function () {
                    debugger;
                }

            });

            //update UI
            //this.newWish is by ref, this.newWish() is by val
            var row = new self.newRow(self.newWish());
            self.wishes.push(row);
            $('#addWishModal').modal('hide');
            self.newWish('');            
        },
        showDelete: function (row) {
            row.hover(true);
        },
        hideDelete: function (row) {
            row.hover(false);
        },
        activate: function () {
            var self = this;
            $.ajax({
                type: "GET",
                url: "/MyList/GetWishlist",
                datatype: 'json',
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    debugger;
                    self.wishes([]);
                    for (var i = 0; i < data.length; i++) {
                        var row = new self.newRow(data[i]);
                        self.wishes.push(row);
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