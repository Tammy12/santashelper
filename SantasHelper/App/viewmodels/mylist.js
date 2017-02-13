define(['service/shared', "toastr"], function (shared, toast) {//took out ko reference
    var vm = {
        shared: shared,

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
                url: "/Wishes/AddNewWish",
                data: { item: self.newWish(), currentUserId:  self.shared.currentUserId},
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
                url: "/Wishes/GetWishlist",
                data: {currentUserId: self.shared.currentUserId},
                datatype: 'json',
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    debugger;
                    self.wishes([]);
                    if (data.success) {
                        for (var i = 0; i < data.names.length; i++) {
                            var row = new self.newRow(data.names[i]);
                            self.wishes.push(row);
                        }
                    }
                    else {
                        toast.error(data.message);
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