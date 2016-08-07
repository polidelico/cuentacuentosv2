/* --------------------------------------------
|  App View
----------------------------------------------*/

App.Views.App = Backbone.View.extend({
    initialize: function () {

        //get general data 
        App.Categories = new App.Collections.Categories(App.CategoriesJson);
        App.Grades = new App.Collections.Grades(App.GradesJson);
        App.PageTypes = new App.Collections.PageTypes(App.PageTypesJson);
        App.Images = new App.Collections.Images(App.ImagesJson);
        App.ImageCategories = new App.Collections.ImageCategories(App.ImagesCategoriesJson);
        //console.table(App.Pages.toJSON());

        //set story properties
        App.Story.set('AllCategories', App.Categories);
        App.Story.set('AllGrades', App.Grades);
        
        // instantiate views
        var saveStoryButtonView = new App.Views.SaveStoryButton();
        var importPDFButtonView = new App.Views.ImportPDF();
        
        var addPageButtonView = new App.Views.AddPageButton();
        var storyInfoModalView = new App.Views.StoryInfoModal({ model: App.Story });
        var imagesModalView = new App.Views.ImagesModal({ collection: App.Images });
        var pageTypesModalView = new App.Views.PageTypesModal({ collection: App.PageTypes });
        var pagesView = new App.Views.Pages({ collection: this.collection }).render();
        var editorView = new App.Views.Editor();
        var imageCategoriesView = new App.Views.ImageCategories({ collection: App.ImageCategories });
        var importPDFModalView = new App.Views.ImportPDFModal

        // FileUpload
        $('#upload-button-wrapper').fileupload({
            url: '/pages/addnewimage/' + App.UserId,
            method: 'POST',
            forceIframeTransport: true,
            start: function (e, data) {
                //showLoading();
            },
            stop: function (e, data) {
                //hideLoading();
            },
            add: function (e, data) {
                data.submit();
            },
            done:function(){
            },
            always: function (e, data) {
                var result = data.result;
                if (result.HasError) {
                    failureFunc(result.Error);
                } else {
                    App.vent.trigger('reloadUserImages');
                }
            }
        });

        //Navigation slider
        App.Globals = {
            sliderInstance: null,
            slider: function (initSlide) {
                return $('.paginas .section2 .slider ul').slick({
                    'slide': 'li',
                    prevArrow: $('.paginas .section2 .rows .left'),
                    nextArrow: $('.paginas .section2 .rows .right'),
                    infinite: false,
                    slidesToShow: 6,
                    slidesToScroll: 2,
                    variableWidth: true,
                    initialSlide: initSlide ? initSlide : 0
                })
            },
            isNewSlide: false
        }

        App.Globals['sliderInstance'] = App.Globals.slider();

        //Support maxlength for ie9
        $(function () {
            $("textarea[maxlength]").bind('input propertychange', function () {
                var maxLength = $(this).attr('maxlength');
                if ($(this).val().length > maxLength) {
                    $(this).val($(this).val().substring(0, maxLength));
                }
            })
        });

        // Navigation sortable        
        //$("#pages-nav ul.pages").sortable({
        //    vertical: false,
        //    onDrop: function ($item, container, _super, event) {
        //        //console.log(container);
        //        //console.log(_super);
        //    }
        //});        

        

        // Global Events
        App.vent.on('addPage', this.addPage, this);

    },
    addPage: function (pageTypeModel) {

        var typeName = pageTypeModel.get('name');
        var image;

        switch (typeName) {
            case 'ImageTopTextBottom':
                image = 'http://placehold.it/390x280&text=cambiar%20imagen';
                break;
            case 'TextTopImageBottom':
                image = 'http://placehold.it/390x280&text=cambiar%20imagen';
                break;
            case 'BigImage':
                image = 'http://placehold.it/390x558&text=cambiar%20imagen';
                break;
            case 'BigImageTextOverlay':
                image = 'http://placehold.it/390x558&text=cambiar%20imagen';
                break;
            default:
                image = '';
                break;
        }

        App.Globals.isNewSlide = true;
        this.collection.add({
            Type: typeName,
            Text: '<h2> Tu texto va aqui </h2>',
            ImageId: 0,
            ImageUrl: image,
            Position: this.collection.length
        });

        App.Pages.savePages();
        
    }

});


/* --------------------------------------------
|  Pages View - Navigation
----------------------------------------------*/
App.Views.Pages = Backbone.View.extend({
    el: '#pages-nav ul.pages',
    currentSet: 0,
    initialize: function () {
        this.collection.on('add', this.addOne, this);
        this.collection.on('destroy', this.deletePage, this);
        this.collection.on('change', this.modelChange, this);
        this.collection.on('saveDone', this.validateSave, this);
        App.vent.on('fixNavigationOnDelete', this.fixOnDelete, this);
    },
    render: function () {
        
        this.collection.each(this.addOne, this);

        $('#pages-nav ul.pages img').each(function (i, e) {

            var $e = $(e);
            $e.on('load', function () {
                var width = this.width;
                var newWidth = width / 3.48;
                $(this).css({ 'width': newWidth, 'margin': '0 auto' });
            });
        });

        return this;

    },    
    addOne: function (page) {

        var pageView = new App.Views.Page({ model: page });
        var html = pageView.render().el;
        this.$el.append(html);

        if (App.Globals && App.Globals.isNewSlide) {

            var numberOfSets = (App.Pages.length / 2) % 1 != 0 ? Math.round(App.Pages.length / 2) : App.Pages.length / 2;
            var navigationSet = this.currentSet > 2 && numberOfSets > 3 ? (this.currentSet * 2) - 2 : 0;
            App.Globals.isNewSlide = false;
            App.Globals.sliderInstance.unslick();
            App.Globals['sliderInstance'] = App.Globals.slider(navigationSet);
            App.vent.trigger('refreshPages');

        }

    },
    deletePage: function (object) {

        this.collection.savePages();
        App.Globals.isDeleted = true;

    },
    modelChange: function () {

        this.collection.savePages();
        var navigationSet = this.currentSet > 2 ? (this.currentSet * 2) - 2 : 0;
        App.Globals.sliderInstance.unslick();
        this.$el.empty();        
        this.render();
        App.Globals['sliderInstance'] = App.Globals.slider(navigationSet);
        
    },
    validateSave: function (status) {

        if (!status)
        {
            alert('Oops, something went wrong, please try again');
            //TODO: reload the collection
            location.reload();
        }
        else {
            //console.log('jobs done');
        }
    },
    fixOnDelete: function () {

        var currentNavSet = (App.Globals.sliderInstance.slickCurrentSlide() / 2) + 1;
        //console.log('nav: ' + currentNavSet + ', currentPageSet: ' + this.currentSet);

        if (this.currentSet == currentNavSet) {

            var navigationSet = ((this.currentSet - 1) * 2) - 2;
            App.Globals.sliderInstance.slickGoTo(navigationSet);

        }

        App.Globals.isDeleted = false;
    }
});

/* --------------------------------------------
|  Page View - Navigation
----------------------------------------------*/
App.Views.Page = Backbone.View.extend({
    tagName: 'li',
    template: getTemplate('pages-nav-template'),
    initialize: function () {
        this.model.on('destroy', this.remove, this);
        this.model.on('change', this.refresh, this);
    },
    refresh: function () {
        this.render();
    },
    events: {
        'click a.page' : 'bringToEditor',
        'click .delete': 'deletePage'        
    },
    bringToEditor: function (e) {

        e.preventDefault();
        var index = App.Pages.indexOf(this.model);
        var editCollection;
        var setToDisplay = 0;

        if (index > 1 && index % 2 == 0) {
            setToDisplay = (index / 2);
        } else if (index > 1 && index % 2 != 0) {
            setToDisplay = (index - 1) / 2;
        }

        App.vent.trigger('changePages', setToDisplay);

    },
    deletePage: function () {

        var environment = this;

        swal({
            title: "¿Quieres eliminar la página seleccionada?",
            text: "Una vez eliminada no podrá ser recuperada.",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Si, bórrala!",
            closeOnConfirm: false
        },
        function () {

            environment.model.destroy({
                success: function () {
                    App.vent.trigger('refreshPages');
                    swal("Borrada!", "Tu página ha sido borrada.", "success");
                    App.vent.trigger('fixNavigationOnDelete');
                }
            });

            
        });

    },    
    render: function () {

        var source = Handlebars.compile(this.template);
        this.setElement(source(this.model.toJSON()));
        return this;

    }
});


/* --------------------------------------------
|  Editor View
----------------------------------------------*/
App.Views.Editor = Backbone.View.extend({
    el: '#pages-designer',
    currentSet: 0,
    events:{
        'click .btn_prev': 'showPrevPages',
        'click .btn_next': 'showNextPages'
    },
    initialize: function () {
        this.render();
        App.vent.on('changePages', this.changePages, this);
        App.vent.on('refreshPages', this.refreshPages, this);
    },
    render: function (setToDisplay) {

        var numberOfSets = (App.Pages.length / 2) % 1 != 0 ? Math.round(App.Pages.length / 2) : App.Pages.length / 2;
        setToDisplay = setToDisplay != null ? setToDisplay : this.currentSet;

        // prevent the editor to be empty if the last page is deleted
        setToDisplay = setToDisplay > numberOfSets - 1 ? setToDisplay - 1 : setToDisplay;

        this.currentSet = setToDisplay;
        App.Views.Pages.prototype.currentSet = setToDisplay;

        var firstPage = this.currentSet * 2;
        var pagesToEditCollection = this.currentSet == 0 ? App.Pages.slice(this.currentSet, this.currentSet + 2)
            : App.Pages.slice(firstPage, firstPage + 2);

        this.$('ul.pages li').empty();
        _.each(pagesToEditCollection, this.addOne, this);
        CKEDITOR.disableAutoInline = true;
        $('#pages-designer div[contenteditable="true"]').ckeditor();
        console.log(this.currentSet);

        return this;

    },
    changePages: function (setToDisplay) {
        this.render(setToDisplay);
    },
    addOne: function (page, index) {

        var cssClass = 'fleft';
        var editorPage = new App.Views.EditorPage({ model: page });

        editorPage.positionClass = cssClass;
        this.$('ul.pages li').append(editorPage.render().el);

    },
    showPrevPages: function (e) {

        e.preventDefault();

        if (this.currentSet > 0) {

            var setToRender = this.currentSet - 1;
            this.render(setToRender);
            
            if (setToRender > 0)
                App.Globals.sliderInstance.slickGoTo((setToRender * 2) - 2);

        }

    },
    showNextPages: function (e) {
        
        e.preventDefault();
        //if the division have decimal places round it 
        var numberOfSets = (App.Pages.length / 2) % 1 != 0 ? Math.round(App.Pages.length / 2) : App.Pages.length / 2;

        if (this.currentSet < numberOfSets - 1) {

            var setToRender = this.currentSet + 1;
            this.render(setToRender);

            if (setToRender > 1 && (numberOfSets > setToRender + 1)) {
                var sliderIndex = (setToRender * 2) - 2;
                App.Globals.sliderInstance.slickGoTo(sliderIndex);
            }

        }

    },
    refreshPages: function () {
        this.render();
    }
});

/* --------------------------------------------
|  EditorPage View
----------------------------------------------*/
App.Views.EditorPage = Backbone.View.extend({
    template: getTemplate('pages-designer-page'),
    events: {
        'blur div[contenteditable="true"]': 'saveText',
        'click img': 'openModal',
        'mouseenter': 'addChangeImgBtn',
        'mouseleave': 'removeChangeImgBtn',
        'click div.changeImageIcon': 'openModal',
        //'keyup div.text': 'validateLength'
    },
    render: function () {

        var source = Handlebars.compile(this.template);
        this.setElement(source({ page: this.model.toJSON(), cssclass: this.positionClass }));
        return this;

    },
    saveText: function (e) {

        //this.validateLength(e);

        var newText = this.$('div[contenteditable="true"]').html();
        this.model.set('Text', newText);

    },
    openModal: function (e) {

        $.fancybox({
            autoSize: true,
            'transitionIn': 'elastic',
            'transitionOut': 'elastic',
            'speedIn': 500,
            'speedOut': 300,
            'href': '#images-modal'
        });

        var $target = $(e.currentTarget);

        if ($target.hasClass('changeImageIcon'))
            App.Views.Image.prototype.imgToReplace = $target.siblings('div.image').find('img');
        else
            App.Views.Image.prototype.imgToReplace = $target;

        App.Views.Image.prototype.pageModel = this.model;

    },
    addChangeImgBtn: function () {
        if (this.$el.hasClass('BigImageTextOverlay')) {
            this.$el.append('<div class="changeImageIcon"> <span class="glyphicon glyphicon-picture" aria-hidden="true"></span> </div>')
        }
    },
    removeChangeImgBtn: function () {
        if (this.$el.hasClass('BigImageTextOverlay')) {
            this.$('.changeImageIcon').remove();
        }
    },
    validateLength: function (e) {

        var $target = $(e.currentTarget);
        var containerHeight = $target.height();
        var textHeight = 0;

        $target.contents().not("h1, h2, h3, h4, h5, h6, p").remove();
        
        $target.children().each(function (i, e) {

            var $e = $(e);
            textHeight += $e.height();

            if (textHeight > (containerHeight + 10)) {
                $e.remove();
            }
            else if (textHeight > (containerHeight - 6)) {

                if ($target.children().length == (i + 1)) {

                    var currentText = '';
                    if ($e.children().length > 0) {

                        currentText = $e.children().last().text();
                        //console.log(currentText)

                        if(currentText)
                            $e.children().last().text(currentText.substring(0, currentText.length - 1));
                        else
                            $e.children().last().remove()

                    } else {

                        currentText = $e.text();
                        $e.text(currentText.substring(0, currentText.length - 1));

                    }

                } else {
                    $e.remove();
                }
            }

        });

        //console.log('cumulative height: ' + textHeight)
        //console.log('container height: ' + containerHeight)
        
    }
});

/* --------------------------------------------
|  SaveStoryButton View (OpenModal)
----------------------------------------------*/
App.Views.SaveStoryButton = Backbone.View.extend({
    el: '#save-story',
    initialize: function () {

        this.$el.fancybox({
            maxWidth: 500,
            autoSize: true,
            closeClick: false,
            openEffect: 'none',
            closeEffect: 'none'
        });

    },
    events: {
        'click': 'openModal'
    },
    openModal: function (e) {
        e.preventDefault();
    }
});

App.Views.ImportPDF = Backbone.View.extend({
    el: '#import-pdf',
    initialize: function () {
        
        this.$el.fancybox({
            width: 520,
            height: 'auto',
            autoSize: false,
            closeClick: false,
        });

    },
    events: {
        'click': 'openModal'
    },
    openModal: function (e) {
        e.preventDefault();
    }
});

App.Views.ImportPDFModal = Backbone.View.extend({
    el: '#import-pdf-modal',
    initialize: function () {
    },
    events: {
        'change #pdf-upload-input': 'fileInputChange',
        'click #remove-file': 'removeFile',
        'click button[type="submit"]': 'showLoadingMsg'
    },
    fileInputChange: function (e) {
        var fileName = this.$('#pdf-upload-input').val();
        this.$('.ui-button-text span').text(fileName);
        this.$('.msgs').hide();
        this.$('.buttons-wrapper').fadeIn(200);
    },
    clickRemoveFile: function (e) {
        e.preventDefault();
        this.removeFile();
    },
    removeFile: function(){        
        var $fileUpload = this.$('#pdf-upload-input');
        $fileUpload.replaceWith($fileUpload.val('').clone(true));
        this.$('.ui-button-text span').text('Seleccionar PDF');
        this.$('.buttons-wrapper').fadeOut(200);
    },
    showLoadingMsg: function (e) {

        e.preventDefault();
        var $fileUpload = this.$('#pdf-upload-input');

        if ($fileUpload.val() && $fileUpload.val().indexOf('.pdf') > 0) {
            
            this.$('.buttons-wrapper').fadeOut(200);
            this.$('.msgs').Loadingdotdotdot({
                "speed": 400,
                "maxDots": 4,
                "word": "Convirtiendo pdf a cuento"
            });
            this.$('form').submit();
        } else {
            console.log('invalid');
            this.$('.msgs').show().text('Archivo invalido');
            this.removeFile();
        }

        
    }
});


/* --------------------------------------------
|  AddPageButton View (OpenModal)
----------------------------------------------*/
App.Views.AddPageButton = Backbone.View.extend({
    el: '#add-page',
    initialize: function () {

        $(".fancybox-templates").fancybox({
            autoSize: true,
            closeClick: false,
            openEffect: 'none',
            closeEffect: 'none'
        });

    },
    events: {
        'click': 'openModal'
    },
    openModal: function (e) {
        e.preventDefault();
    }
});

/* --------------------------------------------
|  StoryInfoModal View
----------------------------------------------*/
App.Views.StoryInfoModal = Backbone.View.extend({
    el: '#story-details-modal',
    template: getTemplate('story-details-content'),
    events:{
        'click #submit-details': 'saveStory'
    },
    initialize: function () {
        this.render();
    },
    render: function () {

        var source = Handlebars.compile(this.template);
        var html = source({
            story: this.model.toJSON(),
            allCategories: this.model.get('AllCategories').toJSON(),
            allGrades: this.model.get('AllGrades').toJSON(),
        });
        this.$el.html(html);

    },
    saveStory: function (e) {

        e.preventDefault();

        var selCategories = [];
        var selGrades= [];
        var name = this.$('#story-name').val();
        var summary = this.$('#story-summary').val();
        var categories = this.$('input[name="category"]:checked');
        var grades = this.$('input[name="grade"]:checked');

        categories.each(function (i, e) {
            selCategories[i] = $(this).val();
        });
        grades.each(function (i, e) {
            selGrades[i] = $(this).val();
        });

        $.ajax({
            type: "POST",
            url: '/stories/save/' + App.Pages.storyId,
            data: {
                name: name,
                summary: summary,
                selectedCategories: JSON.stringify(selCategories),
                selectedGrades: JSON.stringify(selGrades),
            },
            success: function (response) {

                $.fancybox.close(true);

                if (response) {

                    swal({
                        title: "Cambios guardados",
                        type: "success",
                        confirmButtonColor: "#33AEAE"
                    });

                } else {

                    swal({
                        title: "Oops...",
                        text: "¡Algo salió mal!",
                        type: "error",
                        confirmButtonColor: "#33AEAE"
                    });

                }
                
            },
            error: function (response) {

                swal({
                    title: "Oops...",
                    text: "¡Algo salió mal!",
                    type: "error",
                    confirmButtonColor: "#33AEAE"
                });

            }
        });

        //var itemId = $(e.currentTarget).data('id');
        //var itemModel = this.collection.at(itemId);
        //App.vent.trigger('addPage', itemModel);
        //$('#template-selector-modal').trigger('closeModal');

    }
});

/* --------------------------------------------
|  ImagesModal View
----------------------------------------------*/
App.Views.ImagesModal = Backbone.View.extend({
    el: '#images-modal',
    url:'',
    initialize: function () {

        this.render();
        App.vent.on('filterImages', this.filterDefaultImages, this);
        App.vent.on('reloadUserImages', this.reloadUserImages, this);
        
    },
    render: function () {
        
        this.renderDefaultImages();
        this.renderUserImages();

    },
    renderDefaultImages: function (imageCategoryId) {
    
        var defaultImages;
        this.$('#default-imgs').html('');

        if (imageCategoryId)
            defaultImages = this.collection.search({ belongToUser: false, category: String(imageCategoryId) });
        else
            defaultImages = this.collection.search({ belongToUser: false });

        //console.table(defaultImages.toJSON());

        defaultImages.each(this.addOne, this);
        
    },
    renderUserImages: function () {

        //console.table(this.collection.toJSON());
        var userImages = this.collection.search({ belongToUser: true });
        this.$('#user-imgs').html('');

        userImages.each(this.addOne, this);

    },
    addOne: function (image, index) {

        var elem;
        var imageView = new App.Views.Image({ model: image });

        if (image.get('belongToUser') == false)
            elem = $('#default-imgs');
        else
            elem = $('#user-imgs');

        elem.append(imageView.render().el);

    },
    filterDefaultImages: function (imageCategoryModel) {
        this.renderDefaultImages(imageCategoryModel.get('Id'));
    },
    reloadUserImages: function () {

        var environment = this;

        this.collection.fetch({
            success: function (imagesCollection, response, options) {
                environment.renderUserImages();
            }
        });
        
    }
});

/* --------------------------------------------
|  Image View
----------------------------------------------*/
App.Views.Image = Backbone.View.extend({
    imgToReplace: null,
    pageModel: null,
    template: getTemplate('image-template'),
    events: {
        'click img': 'moveandSaveImg',
        'click .delete':'deleteImg'
    },
    render: function () {

        var source = Handlebars.compile(this.template);
        var html = source({ image: this.model.toJSON() });

        this.setElement(html);
        return this;

    },
    moveandSaveImg: function () {

        var environment = this;
        $.ajax({
            type: "POST",
            url: '/pages/moveandsaveimage/' + App.Pages.storyId,
            data: { imgJsonStr: JSON.stringify(this.model.toJSON()) },
            success: function (response) {

                environment.imgToReplace.attr({ 'src': response.imagePath });
                environment.pageModel.set({ 'ImageUrl': response.imagePath, 'ImageId': response.id });
                $.fancybox.close(true)
                //$('#images-modal').trigger('closeModal');
                
            }
        });

    },
    deleteImg: function () {

        var environment = this;

        swal({
            title: "¿Quieres eliminar la imagen seleccionada?",
            text: "Una vez eliminada no podrá ser recuperada.",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Si, bórrala!",
            closeOnConfirm: false
        },
        function () {

            console.log('doing');
            $.ajax({
                type: "DELETE",
                url: '/pages/deleteimage/' + environment.model.get('id'),
                success: function (response) {

                    if (response) {
                        environment.$el.remove();
                        swal("Borrada!", "La imagen ha sido borrada.", "success");
                    }


                }
            });

        });

    }
});

/* --------------------------------------------
|  ImagesCategories View
----------------------------------------------*/
App.Views.ImageCategories = Backbone.View.extend({
    el: '#imageCategories',
    initialize: function () {
        this.render();
    },    
    render: function () {
        
        this.addOne(new App.Models.ImageCategory({Name:'Todas'}));
        this.collection.each(this.addOne, this);
    },
    addOne: function (category, index) {
        var imageCategory = new App.Views.ImageCategory({ model: category });
        this.$el.append(imageCategory.render().el);
        return this;
    }
    
});

/* --------------------------------------------
|  ImagesCategory View
----------------------------------------------*/
App.Views.ImageCategory = Backbone.View.extend({
    template: getTemplate('image-category-template'),
    events:{
        'click' : 'filterImages'
    },
    render: function () {

        var source = Handlebars.compile(this.template);
        var html = source({ category: this.model.toJSON()});
        this.setElement(html);
        return this;

    },
    filterImages: function () {
        App.vent.trigger('filterImages', this.model);
    }

});

/* --------------------------------------------
|  AddPageModal View
----------------------------------------------*/
App.Views.PageTypesModal = Backbone.View.extend({
    el: '#template-selector-modal',
    template: getTemplate('template-selector-content'),
    events: {
        'click tr a': 'addPage'
    },
    initialize: function () {
        this.render();
    },
    render: function () {

        var chunks = App.Functions.divideInChunks(this.collection.toJSON(), 4);
        //console.log(chunks);
        var source = Handlebars.compile(this.template);
        var html = source({ pageTypes: chunks });
        this.$('table').html(html);

    },
    addPage: function (e) {

        e.preventDefault();

        var itemId = $(e.currentTarget).parent('td').data('id');
        var itemModel = this.collection.findWhere({ 'id': itemId });
        App.vent.trigger('addPage', itemModel);
        $.fancybox.close(true);

    }
});