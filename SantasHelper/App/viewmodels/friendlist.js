define(['service/shared', "toastr"], function (shared, toast) {
    var vm = {
        shared: shared,

        wishes: ko.observableArray([]),
        newRow: function (id, name) {
            debugger;
            this.name = name;
            this.id = id;
        },

        modalIndex: null,
        modalTitle: ko.observable(""),
        modalDescription: ko.observable(""),
        activate: function(id)
        {
            var self = this;
            self.wishes([]);
            $.ajax({
                type: "GET",
                url: "/Wishes/GetWishlist",
                data: { currentUserId: id },
                datatype: 'json',
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    debugger;
                    if (data.success) {
                        for (var i = 0; i < data.names.length; i++) {
                            var row = new self.newRow(data.ids[i], data.names[i]);
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
        },
        selectRowModal: function (index) {
            debugger;
            var self = this;
            self.modalIndex = index;
            self.modalTitle(self.wishes()[index].name);
            self.modalDescription("Loading...");
            
            $.ajax({
                type: "GET",
                url: "/Wishes/GetWishDescription",
                data: { wishid: self.wishes()[index].id },
                datatype: 'json',
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    debugger;
                    if (data.success) {
                        if (data.description == null || data.description == "") {
                            self.modalDescription("Up to you. This person isn't picky!");
                        }
                        else {
                            self.modalDescription(data.description);
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
        },
        claimWish: function () {
            debugger;
            var self = this;

            $.ajax({
                type: "GET",
                url: "/Wishes/ClaimWish",
                data: { wishid: self.wishes()[self.modalIndex].id, currentUserId: self.shared.currentUserId },
                datatype: 'json',
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    debugger;
                    if (data.success) {
                        toast.success(data.message);
                        $('#wishDetails').modal('hide');
                    }
                    else {
                        toast.error(data.message);
                    }
                },
                error: function () {
                    debugger;
                    toast.error("Error: unsuccessful claim.");
                }

            });
        }
    };
    return vm;
});