define(['service/shared', 'toastr'], function (shared, toast) {
    var vm = {
        shared: shared,        

        displayName: 'My Shopping List',
        gifts: ko.observableArray([]),
        newRow: function(friend, wish, desc, count) {
            this.friend = friend;
            this.wish = wish;
            this.description = desc;
            this.count = count;
        },
        modalRowIndex: ko.observable(),
        activate: function () {
            debugger;
            var self = this;
            self.gifts([]);
            $.ajax({
                type: "GET",
                url: "/Wishes/GetClaimedList",
                data: { currentUserId: self.shared.currentUserId },
                datatype: 'json',
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    debugger;
                    if (data.success) {
                        for (var i = 0; i < data.wishes.length; i++) {
                            var row = new self.newRow(data.friends[i], data.wishes[i], data.descriptions[i], data.counts[i]);
                            self.gifts.push(row);
                        }
                    }
                    else {
                        toast.error(data.message);
                    }

                },
                error: function (data) {
                    debugger;
                }

            });
        }
    };
    return vm;
});