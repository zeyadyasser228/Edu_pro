document.addEventListener('DOMContentLoaded', function() {
  const searchToggle = document.getElementById('searchToggle');
  const searchContainer = document.getElementById('searchContainer');
  const searchInput = searchContainer.querySelector('.search-input');

  searchToggle.addEventListener('click', function(e) {
    e.stopPropagation();
    searchContainer.classList.toggle('active');
    searchToggle.classList.toggle('active');
    if (searchContainer.classList.contains('active')) {
      setTimeout(() => searchInput.focus(), 300);
    }
  });

  // Close search when clicking outside
  document.addEventListener('click', function(event) {
    if (!searchContainer.contains(event.target) && event.target !== searchToggle) {
      searchContainer.classList.remove('active');
      searchToggle.classList.remove('active');
    }
  });

  // Prevent clicks inside the search container from closing it
  searchContainer.addEventListener('click', function(e) {
    e.stopPropagation();
  });

  // Close search on ESC key press
  document.addEventListener('keydown', function(e) {
    if (e.key === 'Escape' && searchContainer.classList.contains('active')) {
      searchContainer.classList.remove('active');
      searchToggle.classList.remove('active');
    }
  });
});