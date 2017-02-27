define(['service/shared', "toastr"], function (shared, toast) {
    var vm = new function() {
        var self = this;
        self.shared = shared;

        self.friendId = null;
        self.wishes = ko.observableArray([]);
        self.newRow = function (id, name, claim) {
            debugger;
            this.name = name;
            this.id = id;
            this.claim = ko.observable(claim);
        };

        //only needs to be observable for disabling claim button
        self.modalIndex = ko.observable(null);
        self.modalTitle = ko.observable("");
        self.modalDescription = ko.observable("");
        self.activate = function(id)
        {
            debugger;
            //var self = this;
            self.friendId = id;
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
                            var row = new self.newRow(data.ids[i], data.names[i], data.claimed[i]);
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
            debugger;
        };
        self.selectRowModal = function (index) {
            debugger;
            //var self = this;
            self.modalIndex(index);
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
        };
        self.claimWish = function () {
            debugger;
            //var self = this;

            $.ajax({
                type: "GET",
                url: "/Wishes/ClaimWish",
                data: { wishid: self.wishes()[self.modalIndex()].id, currentUserId: self.shared.currentUserId },
                datatype: 'json',
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    debugger;
                    if (data.success) {
                        //call activate again to update the .claim for each wish
                        self.activate(self.friendId);
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
        };
        self.enableClaim = ko.computed({
            read: function () {
                debugger;
                if (self.modalIndex() == null || self.wishes().length < self.modalIndex())
                    return false;


                if (self.wishes()[self.modalIndex()].claim())
                    return false;
                else return true;
            },
            deferEvaluation: true
        });
    }();
    return vm;
});