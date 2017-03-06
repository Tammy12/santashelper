define(['service/shared', "toastr"], function (shared, toast) {//took out ko reference
    var vm = {
        shared: shared,

        displayName: 'My Wish List',
        newWish: ko.observable(''),
        newWishDescription: ko.observable(''),
        newWishCount: ko.observable(1),
        wishes: ko.observableArray([]),
        newRow: function (id, title, desc, count) {
            this.id = id;
            this.name = title;
            this.description = desc;
            this.count = count;
            this.hover = ko.observable(false);
        },
        modalItemId: ko.observable(null),
        resetNewWish: function () {
            var self = this;
            self.newWish('');
            self.newWishDescription('');
            self.newWishCount(1);
        },
        displayRowDetails: function () {
            debugger;
            var self = this;
            var rowIndex = self.modalItemId();
            self.newWish(self.wishes()[rowIndex].name);
            self.newWishDescription(self.wishes()[rowIndex].description);
            self.newWishCount(self.wishes()[rowIndex].count);
        },

        deleteWish: function (index) {
            debugger;
            var self = this;
            $.ajax({
                type: "GET",
                url: "/Wishes/DeleteWish",
                data: {itemId: self.wishes()[index].id},
                datatype: 'json',
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    debugger;
                    if (data.success)
                        self.activate();
                   else toast.error(data.message);
                },
                error: function (data) {
                    debugger;
                }

            });
        },
        validateInput: function() {
            debugger;
            var self = this;
            if (self.newWish() == null || self.newWish() == "") {
                toast.warning("Your item is missing a title.");
                return false;
            }

            if (self.newWishCount() == null || self.newWishCount() == "" || isNaN(parseInt(self.newWishCount(), 10)) || parseInt(self.newWishCount(), 10) < 1) {
                toast.warning("Item count must be a positive integer.");
                return false;
            }
        },
        editWish: function () {
            debugger;
            var self = this;
            if (self.validateInput() == false) {
                return;
            }

            if (self.modalItemId() == null)
                self.addNewWish();
            else {
                self.editExistingWish(self.wishes()[self.modalItemId()].id);
            }
        },
        editExistingWish: function (itemId) {
            var self = this;
            var sendData = {
                itemId: itemId,
                itemName: self.newWish(),
                itemDesc: self.newWishDescription(),
                itemCount: self.newWishCount(),
                currentUserId: self.shared.currentUserId
            };
            //update mysql database
            $.ajax({
                type: "GET",
                url: "/Wishes/EditExistingWish",
                data: sendData,
                datatype: 'json',
                contentType: "application/json; charset=utf-8",
                success: function () {
                    debugger;
                    self.modalItemId(null);
                    self.activate();
                },
                error: function () {
                    debugger;
                    self.modalItemId(null);
                }

            });

            $('#addWishModal').modal('hide');
        },
        addNewWish: function () {
            debugger;
            var self = this;
            var sendData = {
                itemName: self.newWish(),
                itemDesc: self.newWishDescription(),
                itemCount: self.newWishCount(),
                currentUserId: self.shared.currentUserId
            };

            //update mysql database
            $.ajax({
                type: "GET",
                url: "/Wishes/AddNewWish",
                data: sendData,
                datatype: 'json',
                contentType: "application/json; charset=utf-8",
                success: function () {
                    debugger;
                    //update UI
                    //this.newWish is by ref, this.newWish() is by val
                    self.activate();
                    $('#addWishModal').modal('hide');
                    self.newWish('');
                    self.newWishDescription('');
                    self.newWishCount(1);
                },
                error: function () {
                    debugger;
                }

            });                       
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
                            var row = new self.newRow(data.ids[i], data.names[i], data.descriptions[i], data.counts[i]);
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