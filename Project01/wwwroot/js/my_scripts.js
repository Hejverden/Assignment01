document.addEventListener('DOMContentLoaded', (event) => {
    window.currentPage = 1;
    window.currentSearchTerm = '';
    window.isLoading = false;
    window.currentSorting = "Relevant";

    // ************************************************************************************************
    // Handling/activating search if the following occurs: 
    // Enter key press on the search input (see above)
    // User presses the search button
    window.handleSearch = function() 
    {
        window.currentSearchTerm = document.getElementById('searchTerm').value;
        window.currentPage = 1;
        const sorting = document.getElementById("sorting").value;
        document.getElementById('photoGallery').innerHTML = ''; // Clear previous results
        if (window.currentSearchTerm) 
        {
            fetchPhotos(window.currentSearchTerm, window.currentPage, sorting);
        } 
        else 
        {
            fetchPhotos("", window.currentPage, sorting);
        }
        updateHistory();
    }

    // Add event listener for Enter key press on the search input
    const searchInput = document.getElementById('searchTerm');
    searchInput.addEventListener('keydown', (event) => 
    {
        if (event.key === 'Enter') 
        {
            event.preventDefault(); // Prevent form submission if inside a form
            window.handleSearch();
        }
    });

    // ************************************************************************************************
    // Search category: Relevance, Data uploaded, Date taken, Interesting
    window.handleSortingChange = function () 
    {
        window.currentSearchTerm = document.getElementById('searchTerm').value;
        window.currentPage = 1;
        const sorting = document.getElementById("sorting").value;
        document.getElementById('photoGallery').innerHTML = ''; // Clear previous results
        fetchPhotos(window.currentSearchTerm, window.currentPage, sorting);
        window.currentSorting = sorting;
    }

    // ************************************************************************************************
    // Fetching photos by sending API request to the applications REST API
    window.fetchPhotos = function(searchTerm, page, sorting) 
    {
        if (window.isLoading) return; // Prevent multiple simultaneous requests
        window.isLoading = true;

        const loading = document.getElementById('loading');
        const error = document.getElementById('error');
        
        // When loading starts, display the loading spinner and hide the error message
        loading.style.display = 'block';
        loading.classList.add('active');
        error.style.display = 'none';

        // Set a timeout for the fetch request
        const controller = new AbortController();
        const timeoutId = setTimeout(() => 
        {
            controller.abort(); // Abort the fetch request
            // When loading finishes or fails
            loading.classList.remove('active');
            loading.style.display = 'none';
            error.classList.add('active'); // On error
            error.textContent = 'Request timed out. Please try again later.';
            error.style.display = 'block';
            window.isLoading = false; // Reset loading state
        }, 10000); // Timeout after 10 seconds

        let url = "";
        // If users insert nothing or a whitespace, we will search for null: that case our REST API will send a request to Flickr.photos.getRecent API instead.
        // NB: My first take was to send a different API requests to my REST API serverside. In that case, I would have had two different endpoints (controllers),
        // But I choose to handle both types of request in one endpoint. For now, I find that more efficient, since I save some code.
        if (!searchTerm || searchTerm.trim() === "")
        {
            console.log("Search term: NULL");
            url = `/api/photos/search?searchTerm=null&page=${encodeURIComponent(page)}&sort=${encodeURIComponent(sorting)}`;
            document.getElementById('sorting').value = "Date uploaded";
            console.log("Our REST API URL:", url);
            var  note = "NB: Entering a whitespace or no search term will return the most recent photos from Flickr!";
            document.getElementById('note').innerHTML = note;
            document.getElementById('note').style.display = "block";
        } 
        else
        {
            console.log("Search term:", searchTerm);
            url = `/api/photos/search?searchTerm=${encodeURIComponent(searchTerm)}&page=${encodeURIComponent(page)}&sort=${encodeURIComponent(sorting)}`;
            console.log("Our REST API URL:", url);
            document.getElementById('note').innerHTML = "";
            document.getElementById('note').style.display = "none";
        }
        fetch(url)
            .then(response => 
            {
                clearTimeout(timeoutId); // Clear the timeout if request is successful
                if (!response.ok) 
                {
                    console.log("response:", response);
                    // Extract the error message from the response body
                    return response.text().then(text => 
                        {
                            throw new Error(text);
                        });
                }
                return response.json();
            })
            .then(data => 
            {
                loading.style.display = 'none';
                renderPhotos(data);
                window.isLoading = false; // Reset loading state
            })
            .catch((err) => 
            {
                console.log(err.message);
                let partsByStart = err.message.split("Start");
                let partsByEnd = partsByStart[1].split("End");
                let finalMessage = partsByEnd[0];
                console.log("Final message:", finalMessage);
                loading.style.display = 'none';
                error.innerHTML = `<p><span style="color: red;">${finalMessage}</span><br><span style="color: black;"></span></p>`;
                error.style.display = 'block';
                window.isLoading = false; // Reset loading state
            }
        );
    }

    // ************************************************************************************************
    // Displaying photos on the page
    window.renderPhotos = function(photos) 
    {
        const photoGallery = document.getElementById('photoGallery');
        console.log(photos);
        photos.forEach(photo => 
        {
            console.log(photo);
            const photoDiv = document.createElement('div');
            photoDiv.className = 'photo';
            const img = document.createElement('img');
            img.src = photo.imageUrl;
            img.alt = photo.title;
            const title = document.createElement('p');
            title.textContent = photo.title;
            photoDiv.appendChild(img);
            // photoDiv.appendChild(title);  // Perhaps nicer without title !?
            photoGallery.appendChild(photoDiv);
        });
    }

    // ************************************************************************************************
    // Endless scroll implementation
    const photoGalleryContainer = document.querySelector('.photo-gallery-container');
    photoGalleryContainer.addEventListener('scroll', () => 
    {
        if ((photoGalleryContainer.scrollTop + photoGalleryContainer.clientHeight) >= photoGalleryContainer.scrollHeight - 100 && !window.isLoading) 
        {
            window.currentPage++;
            window.currentSearchTerm = document.getElementById('searchTerm').value;
            fetchPhotos(window.currentSearchTerm, window.currentPage, window.currentSorting);
        }
    });

    // ************************************************************************************************
    // Showing recent search
    window.updateHistory = function()
    {
        const td_text_container = document.getElementById('td_text_container');
        // Clear the existing content
        td_text_container.innerHTML = '';

        // Create the table element
        const table = document.createElement('table');

        // Create the first tr element
        const tr1 = document.createElement('tr');
        const td1 = document.createElement('td');
        td1.textContent = 'Recent Search';
        tr1.appendChild(td1);

        // Create the second tr element
        const tr2 = document.createElement('tr');
        const td2 = document.createElement('td');

        // Create the div element with id 'text_container'
        const textContainerDiv = document.createElement('div');
        textContainerDiv.id = 'text_container';
        textContainerDiv.style.border = '1px solid black';

        var searchHistoryurl = `/api/searchHistory/show`;
        fetch(searchHistoryurl)
            .then(response => 
            {
                if (response.ok) 
                {
                    console.log("Search term response:", response);
                    return response.json();
                }
            })
            .then(data => 
            {
                data.forEach(searchTermObj => 
                {
                    const termButton = document.createElement('button');
                    termButton.className = 'search-term';
                    termButton.textContent = searchTermObj.queryText;
                    textContainerDiv.appendChild(termButton);

                    termButton.onclick = function() {
                        document.getElementById('searchTerm').value = searchTermObj.queryText;
                        handleSearch();
                    };
                });
            })
            .catch((err) => 
            {
            }
        );

        td2.appendChild(textContainerDiv);
        tr2.appendChild(td2);
        
        table.appendChild(tr1);
        table.appendChild(tr2);

        td_text_container.appendChild(table);
    }
});
