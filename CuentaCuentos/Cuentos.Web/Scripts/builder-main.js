
App.Story = new App.Models.Story(App.StoryJson);
App.Pages = new App.Collections.Pages();


App.Functions = {
    divideInChunks: function (array, size) {

        var results = [];
        while (array.length) {
            results.push(array.splice(0, size));
        }
        return results;
    }
}


App.Pages.fetch({
    success: function (collection, response, options) {
        new App.Views.App({ collection: App.Pages });
    }
});