define([], function () {
    var vm = {
        wishes: ko.observableArray([]),
        newRow: function (id, name) {
            debugger;
            this.name = name;
            this.id = id;
        },

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
            self.modalTitle(self.wishes()[index].name);
            
            //$.ajax({
            //    type: "GET",
            //    url: "/Wishes/GetWishDescription",
            //    data: { currentUserId: id },
            //    datatype: 'json',
            //    contentType: "application/json; charset=utf-8",
            //    success: function (data) {
            //        debugger;
            //        for (var i = 0; i < data.length; i++) {
            //            self.wishes.push(data[i]);
            //        }
            //    },
            //    error: function () {
            //        debugger;
            //    }

            //});
        },
        claimWish: function () {
            var self = this;
            debugger;
        }
    };
    return vm;
});