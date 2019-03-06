using MvvmCross.Forms.Views;
using MvvmCross.Forms.Presenters.Attributes;
using UpcomingMovies.Core.ViewModels;
using Xamarin.Forms;
using FFImageLoading.Forms;
using Xamarin.Essentials;
using UpcomingMovies.Forms.UI.ValueConverters;
using UpcomingMovies.Forms.UI.Effects;
using System;
using AiForms.Renderers;
using UpcomingMovies.Forms.UI.Templates;

namespace UpcomingMovies.Forms.UI.Views
{
    [MvxContentPagePresentation]
    public partial class MovieDetailPage : MvxContentPage<MovieDetailViewModel>
    {
        CachedImage backdropImage;

        int currentBackdropCalculatedHeight;
        int backdropMinHeight = 200;
        float backdropDesiredHeightPercent = 0.3f;

        public MovieDetailPage()
        {
            InitializeComponent();

            CalculateBackdropSize();
            SetupViews();
            SetupScroll();

            SizeChanged += (sender, e) => CalculateBackdropSize();
        }

        void CalculateBackdropSize()
        {
            DisplayInfo info = DeviceDisplay.MainDisplayInfo;
            double dipSize = 0;

            if (info.Orientation == DisplayOrientation.Portrait)
                dipSize = info.Height / info.Density;
            else
                dipSize = info.Width / info.Density;

            currentBackdropCalculatedHeight = (int)(dipSize * backdropDesiredHeightPercent) <= backdropMinHeight ? backdropMinHeight : (int)(dipSize * backdropDesiredHeightPercent);
        }

        void SetupScroll()
        {
            scrollView.Scrolled += (sender, e) =>
            {
                if (e.ScrollY < 0)
                {
                    backdropImage.Layout(new Rectangle(e.ScrollY, e.ScrollY, scrollView.Width - e.ScrollY * 2, currentBackdropCalculatedHeight - e.ScrollY));
                }
                else
                {
                    backdropImage.Layout(new Rectangle(0, 0, scrollView.Width, backdropImage.Height));
                }
            };
        }

        void SetupViews()
        {
            SetupBackdropImage();

            masterRelativeContainer.Children.Add(backdropImage,
            xConstraint: Constraint.RelativeToParent((parent) =>
            {
                return parent.X;
            }), yConstraint: Constraint.RelativeToParent((parent) =>
            {
                return parent.Y;
            }), widthConstraint: Constraint.RelativeToParent((parent) =>
            {
                return parent.Width;
            }), heightConstraint: Constraint.RelativeToParent((parent) =>
            {
                return currentBackdropCalculatedHeight;
            }));

            var infosRelativeContainer = new RelativeLayout();

            masterRelativeContainer.Children.Add(infosRelativeContainer,
            xConstraint: Constraint.RelativeToParent((parent) =>
            {
                return parent.X;
            }), yConstraint: Constraint.RelativeToParent((parent) =>
            {
                return currentBackdropCalculatedHeight;
            }), widthConstraint: Constraint.RelativeToParent((parent) =>
            {
                return parent.Width;
            }));

            var titleLabel = SetupTitle();
            var popularityLabel = SetupPopularity();
            var ratingLabel = SetupRating();
            var runtimeLabel = SetupRuntime();

            var titleLayout = new StackLayout();
            titleLayout.Margin = new Thickness(20, 10);
            titleLayout.Spacing = 2;
            titleLayout.Children.Add(titleLabel);
            titleLayout.Children.Add(popularityLabel);
            titleLayout.Children.Add(ratingLabel);
            titleLayout.Children.Add(runtimeLabel);

            var coverImage = SetupCoverImage();

            infosRelativeContainer.Children.Add(coverImage,
            xConstraint: Constraint.RelativeToParent((parent) =>
            {
                return parent.X + parent.Width * .04;
            }), yConstraint: Constraint.RelativeToParent((parent) =>
            {
                return 0 - (parent.Width * .42) * .4;
            }), widthConstraint: Constraint.RelativeToParent((parent) =>
            {
                return parent.Width * .3;
            }), heightConstraint: Constraint.RelativeToParent((parent) =>
            {
                return parent.Width * .42;
            }));

            infosRelativeContainer.Children.Add(titleLayout,
            xConstraint: Constraint.RelativeToView(coverImage, (parent, sibling) =>
            {
                return sibling.X + sibling.Width;
            }), yConstraint: Constraint.RelativeToView(coverImage, (parent, sibling) =>
            {
                return 0;
            }), widthConstraint: Constraint.RelativeToView(coverImage, (parent, sibling) =>
            {
                return parent.Width - sibling.X - sibling.Width;
            }));

            var infoStack = new StackLayout
            {
                Margin = new Thickness(0, 12, 0, 0)
            };

            var taglineLabel = SetupTagline();
            infoStack.Children.Add(taglineLabel);

            var overviewHeader = SetupOverviewHeader();
            infoStack.Children.Add(overviewHeader);

            var overviewLabel = SetupOverview();
            infoStack.Children.Add(overviewLabel);

            var detailHeader = SetupDetailHeader();
            infoStack.Children.Add(detailHeader);

            var releaseDateLabel = SetupReleaseDate();
            infoStack.Children.Add(releaseDateLabel);

            var genreLabel = SetupGenres();
            infoStack.Children.Add(genreLabel);

            var videosHeader = SetupVideosHeader();
            infoStack.Children.Add(videosHeader);

            var videosView = SetupVideos();
            infoStack.Children.Add(videosView);

            var castHeader = SetupCastHeader();
            infoStack.Children.Add(castHeader);

            var castCollectionView = SetupCastCollectionView();
            infoStack.Children.Add(castCollectionView);

            infosRelativeContainer.Children.Add(infoStack,
            xConstraint: Constraint.RelativeToView(coverImage, (parent, sibling) =>
            {
                return 0;
            }), yConstraint: Constraint.RelativeToView(coverImage, (parent, sibling) =>
            {
                return coverImage.Height + coverImage.Y;
            }), widthConstraint: Constraint.RelativeToParent((parent) =>
            {
                return parent.Width;
            }));
        }

        void SetupBackdropImage()
        {
            backdropImage = new CachedImage
            {
                Aspect = Aspect.AspectFill,
                CacheType = FFImageLoading.Cache.CacheType.All,
                CacheDuration = TimeSpan.FromDays(30),
                RetryCount = 1,
                RetryDelay = 500,
                LoadingPlaceholder = "movie_placeholder.png",
                ErrorPlaceholder = "movie_placeholder.png"
            };
            backdropImage.SetBinding(CachedImage.SourceProperty, new Binding("Movie.BackdropPath", BindingMode.Default, new MovieImagePathConverter(), 500));
        }

        CachedImage SetupCoverImage()
        {
            var coverImage = new CachedImage
            {
                Aspect = Aspect.AspectFit,
                CacheType = FFImageLoading.Cache.CacheType.All,
                CacheDuration = TimeSpan.FromDays(30),
                RetryCount = 0,
                RetryDelay = 500,
                LoadingPlaceholder = "movie_placeholder.png",
                ErrorPlaceholder = "movie_placeholder.png"
            };
            coverImage.SetBinding(CachedImage.SourceProperty, new Binding("Movie.PosterPath", BindingMode.Default, new MovieImagePathConverter(), 300));

            ShadowEffect.SetEnabled(coverImage, true);
            ShadowEffect.SetShadowRadius(coverImage, 10);
            ShadowEffect.SetShadowXOffset(coverImage, 4);
            ShadowEffect.SetShadowYOffset(coverImage, 4);
            ShadowEffect.SetShadowOpacity(coverImage, 0.5f);
            ShadowEffect.SetShadowColor(coverImage, Color.Black);
            return coverImage;
        }

        Label SetupTitle()
        {
            var titleLabel = new Label
            {
                Margin = new Thickness(0, 0, 0, 5),
                Style = Application.Current.Resources["MovieTitleStyle"] as Style
            };
            titleLabel.FontSize = 13;
            titleLabel.SetBinding(Label.TextProperty, new Binding("Movie.Title", BindingMode.Default));
            return titleLabel;
        }

        Label SetupRating()
        {
            var ratingLabel = new Label
            {
                Style = Application.Current.Resources["MovieDetailStyle"] as Style
            };
            ratingLabel.SetBinding(Label.TextProperty, new Binding("Movie.VoteAverage", BindingMode.Default) { StringFormat = "Rating: {0}"});
            return ratingLabel;
        }

        Label SetupPopularity()
        {
            var popularityLabel = new Label
            {
                Style = Application.Current.Resources["MovieDetailStyle"] as Style
            };
            popularityLabel.SetBinding(Label.TextProperty, new Binding("Movie.Popularity", BindingMode.Default) { StringFormat = "Popularity: {0}"});
            return popularityLabel;
        }

        Label SetupRuntime()
        {
            var runtimeLabel = new Label
            {
                Style = Application.Current.Resources["MovieDetailStyle"] as Style
            };
            runtimeLabel.SetBinding(Label.TextProperty, new Binding("Movie.Runtime", BindingMode.Default, new MovieRuntimeConverter()));
            return runtimeLabel;
        }

        Label SetupTagline()
        {
            var taglineLabel = new Label
            {
                Margin = 18,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                Style = Application.Current.Resources["MovieDetailQuoteStyle"] as Style
            };
            taglineLabel.SetBinding(Label.TextProperty, new Binding("Movie.Tagline", BindingMode.Default) { StringFormat = "\"{0}\""});
            return taglineLabel;
        }

        Label SetupOverview()
        {
            var overviewLabel = new Label
            {
                Margin = new Thickness(20, 8),
                Style = Application.Current.Resources["MovieDetailStyle"] as Style
            };
            overviewLabel.SetBinding(Label.TextProperty, new Binding("Movie.Overview", BindingMode.Default));
            return overviewLabel;
        }

        Label SetupReleaseDate()
        {
            var releaseDateLabel = new Label
            {
                Margin = new Thickness(20, 8, 20, 0),
                Style = Application.Current.Resources["MovieDetailStyle"] as Style
            };
            releaseDateLabel.SetBinding(Label.TextProperty, new Binding("Movie.ReleaseDate", BindingMode.Default, new ReleaseDateConverter()));
            return releaseDateLabel;
        }

        Label SetupGenres()
        {
            var genres = new Label
            {
                Margin = new Thickness(20, 0, 20, 8),
                Style = Application.Current.Resources["MovieDetailStyle"] as Style
            };
            genres.SetBinding(Label.TextProperty, new Binding("Movie.Genres", BindingMode.Default, new GenreListConverter()));
            return genres;
        }

        View SetupOverviewHeader()
        {
            var container = ContainerHeader();
            var overview = new Label
            {
                Text = "Overview",
                Style = Application.Current.Resources["MovieDetailHeaderStyle"] as Style 
            };
            container.Children.Add(overview);
            return container;
        }

        View SetupDetailHeader()
        {
            var container = ContainerHeader();
            var details = new Label
            {
                Text = "Details",
                Style = Application.Current.Resources["MovieDetailHeaderStyle"] as Style 
            };
            container.Children.Add(details);
            return container;
        }

        View SetupVideos()
        {
            var vt = new VideoTemplate();
            vt.SetBinding(BindingContextProperty, new Binding("Movie.VideoResults.Videos", BindingMode.Default, null));
            return vt;
        }

        View SetupCastHeader()
        {
            var container = ContainerHeader();
            var cast = new Label
            {
                Text = "Cast",
                Style = Application.Current.Resources["MovieDetailHeaderStyle"] as Style 
            };
            container.Children.Add(cast);
            return container;
        }

        View SetupVideosHeader()
        {
            var container = ContainerHeader();
            var videos = new Label
            {
                Text = "Videos",
                Style = Application.Current.Resources["MovieDetailHeaderStyle"] as Style 
            };
            container.Children.Add(videos);
            return container;
        }

        HCollectionView SetupCastCollectionView()
        {
            var dataTemplate = new DataTemplate(typeof(CastTemplate));
            var hcv = new HCollectionView
            {
                ItemTemplate = dataTemplate,
                HeightRequest = 170,
                ColumnWidth = 140,
            };
            hcv.SetBinding(HCollectionView.ItemsSourceProperty, new Binding("Movie.Credits.Cast", BindingMode.Default, null));
            return hcv;
        }

        StackLayout ContainerHeader()
        {
            var stack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center,
                HeightRequest = 40,
                BackgroundColor = (Application.Current.Resources["primaryDarkColor"] as Color?) ?? Color.Black,
            };

            return stack;
        }
    }
}
