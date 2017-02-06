var options = new RequestFilteringOptions()
    .AddFileExtensionRequestFilter(new FileExtensionsOptions
    {
        FileExtensionsCollection = new List
        {
            new FileExtensionsElement() { FileExtension = ".jpg", Allowed = true },
            new FileExtensionsElement() { FileExtension = ".psd", Allowed = false }
        }
    })
    .AddHttpVerbRequestFilter(new HttpVerbsOptions
    {
            AllowUnlisted = false,
            HttpVerbsCollection = new List
            {
                 new HttpVerbElement() { Verb = HttpVerb.Get, Allowed = true }
            }
    })
    .AddQueryStringRequestFilter(new QueryStringsOptions
    {
            AllowUnlisted = false,
            QueryStringsCollection = new List
            {
                 new QueryStringElement() { QueryString = "id", Allowed = true },
                 new QueryStringElement() { QueryString = "name", Allowed = false }
            }
    })
    .AddHiddenSegmentRequestFilter(new HiddenSegmentsOptions
    {
            HiddenSegmentsCollection = new List
            {
                 new HiddenSegmentElement() { Segment = "Private" }
            }
    })
    .AddHeaderRequestFilter(new HeadersOptions
    {
            HeadersCollection = new List
            {
                 new HeaderElement() { Header = "X-Auth", SizeLimit = 5 }
            }
    })
    .AddUrlRequestFilter(new UrlsOptions
    {
            DeniedUrlSequences = new[] { "me" },
            AllowedUrls = new[] { "/Home" }
    });

app.UseRequestFilter(options);