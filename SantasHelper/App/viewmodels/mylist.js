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
            //this.newWish is by ref, this.newWish() is by val
            var row = new this.newRow(this.newWish());
            this.wishes.push(row);
            $('#addWishModal').modal('hide');
            this.newWish('');
        },
        showDelete: function (row) {
            row.hover(true);
        },
        hideDelete: function (row) {
            row.hover(false);
        }
    };
    return vm;
});