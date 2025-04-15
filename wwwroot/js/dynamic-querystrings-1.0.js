// Define the jQuery plugin
(function ($) {
    $.getQueryParams = function () {
        var queryParams = {};
        var queryString = window.location.search.substring(1); // Get query string excluding the '?'

        // Split the query string into key-value pairs
        var pairs = queryString.split('&');

        // Loop through each pair and populate the queryParams object
        pairs.forEach(function (pair) {
            var splitPair = pair.split('=');
            var key = decodeURIComponent(splitPair[0]);
            var value = decodeURIComponent(splitPair[1] || '');

            // Check if the key already exists in the object
            if (queryParams.hasOwnProperty(key)) {
                // If the key already exists, convert the value to an array
                if (!Array.isArray(queryParams[key])) {
                    queryParams[key] = [queryParams[key]];
                }
                queryParams[key].push(value);
            } else {
                queryParams[key] = value;
            }
        });

        return queryParams;
    };
})(jQuery);