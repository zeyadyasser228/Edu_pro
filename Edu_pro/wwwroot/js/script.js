document.addEventListener("DOMContentLoaded", function () {
  const courses = [
    {
      id: 1,
      title: "C# Programming Course",
      description:
        "Learn the fundamentals of programming in C# through hands-on projects.",
      image: "/placeholder.svg?height=200&width=300",
      level: "beginner",
      duration: "3 Weeks",
      price: "$29.00",
      rating: 5.0,
      ratingCount: 7,
      lessons: 8,
      students: 20,
      category: "programming",
    },
    {
      id: 2,
      title: "UI UX",
      description:
        "Discover how to design attractive and user-friendly interfaces.",
      image: "/placeholder.svg?height=200&width=300",
      level: "advanced",
      duration: "8 Weeks",
      price: "$49.00",
      rating: 4.5,
      ratingCount: 9,
      lessons: 15,
      students: 35,
      category: "design",
    },
    {
      id: 3,
      title: "Digital Marketing Course",
      description:
        "Learn digital marketing strategies to increase your brand awareness.",
      image: "/placeholder.svg?height=200&width=300",
      level: "intermediate",
      duration: "5 Weeks",
      price: "$35.00",
      rating: 4.9,
      ratingCount: 7,
      lessons: 13,
      students: 18,
      category: "marketing",
    },
    {
      id: 4,
      title: "JavaScript Fundamentals",
      description: "Master the basics of JavaScript programming language.",
      image: "/placeholder.svg?height=200&width=300",
      level: "beginner",
      duration: "4 Weeks",
      price: "$32.00",
      rating: 4.7,
      ratingCount: 12,
      lessons: 10,
      students: 45,
      category: "programming",
    },
    {
      id: 5,
      title: "Python for Data Science",
      description:
        "Learn Python programming for data analysis and visualization.",
      image: "/placeholder.svg?height=200&width=300",
      level: "intermediate",
      duration: "6 Weeks",
      price: "$45.00",
      rating: 4.8,
      ratingCount: 15,
      lessons: 12,
      students: 38,
      category: "programming",
    },
    {
      id: 6,
      title: "Graphic Design Masterclass",
      description:
        "Comprehensive course on graphic design principles and tools.",
      image: "/placeholder.svg?height=200&width=300",
      level: "advanced",
      duration: "10 Weeks",
      price: "$59.00",
      rating: 4.6,
      ratingCount: 11,
      lessons: 20,
      students: 25,
      category: "design",
    },
    {
      id: 7,
      title: "Social Media Marketing",
      description:
        "Learn strategies to grow your business using social media platforms.",
      image: "/placeholder.svg?height=200&width=300",
      level: "beginner",
      duration: "4 Weeks",
      price: "$39.00",
      rating: 4.5,
      ratingCount: 8,
      lessons: 9,
      students: 30,
      category: "marketing",
    },
    {
      id: 8,
      title: "Web Development Bootcamp",
      description: "Comprehensive course on HTML, CSS, and JavaScript.",
      image: "/placeholder.svg?height=200&width=300",
      level: "intermediate",
      duration: "8 Weeks",
      price: "$69.00",
      rating: 4.9,
      ratingCount: 18,
      lessons: 25,
      students: 42,
      category: "programming",
    },
    {
      id: 9,
      title: "Business Analytics",
      description:
        "Learn data analysis techniques for business decision making.",
      image: "/placeholder.svg?height=200&width=300",
      level: "advanced",
      duration: "7 Weeks",
      price: "$55.00",
      rating: 4.7,
      ratingCount: 9,
      lessons: 14,
      students: 22,
      category: "business",
    },
    {
      id: 10,
      title: "Mobile App Development",
      description: "Learn to build mobile applications for iOS and Android.",
      image: "/placeholder.svg?height=200&width=300",
      level: "intermediate",
      duration: "9 Weeks",
      price: "$65.00",
      rating: 4.8,
      ratingCount: 14,
      lessons: 18,
      students: 32,
      category: "programming",
    },
    {
      id: 11,
      title: "Content Marketing Strategy",
      description:
        "Learn to create and distribute valuable content to attract customers.",
      image: "/placeholder.svg?height=200&width=300",
      level: "beginner",
      duration: "5 Weeks",
      price: "$42.00",
      rating: 4.6,
      ratingCount: 10,
      lessons: 11,
      students: 28,
      category: "marketing",
    },
    {
      id: 12,
      title: "Entrepreneurship Fundamentals",
      description:
        "Learn the basics of starting and running a successful business.",
      image: "/placeholder.svg?height=200&width=300",
      level: "beginner",
      duration: "6 Weeks",
      price: "$49.00",
      rating: 4.7,
      ratingCount: 12,
      lessons: 15,
      students: 35,
      category: "business",
    },
  ];

  // DOM elements
  const coursesContainer = document.getElementById("courses-container");
  const categoryFilter = document.getElementById("category");
  const levelFilter = document.getElementById("level");
  const durationFilter = document.getElementById("duration");
  const searchInput = document.getElementById("search");
  const searchBtn = document.getElementById("search-btn");
  const prevPageBtn = document.getElementById("prev-page");
  const nextPageBtn = document.getElementById("next-page");
  const pageNumbers = document.getElementById("page-numbers");

  // Pagination state
  let currentPage = 1;
  const coursesPerPage = 6;
  let filteredCourses = [...courses];

  // Initialize
  renderCourses();
  updatePagination();

  // Event listeners
  categoryFilter.addEventListener("change", applyFilters);
  levelFilter.addEventListener("change", applyFilters);
  durationFilter.addEventListener("change", applyFilters);
  searchBtn.addEventListener("click", applyFilters);
  searchInput.addEventListener("keyup", function (event) {
    if (event.key === "Enter") {
      applyFilters();
    }
  });

  prevPageBtn.addEventListener("click", function () {
    if (currentPage > 1) {
      currentPage--;
      renderCourses();
      updatePagination();
    }
  });

  nextPageBtn.addEventListener("click", function () {
    const totalPages = Math.ceil(filteredCourses.length / coursesPerPage);
    if (currentPage < totalPages) {
      currentPage++;
      renderCourses();
      updatePagination();
    }
  });

  // Functions
  function renderCourses() {
    coursesContainer.innerHTML = "";

    const startIndex = (currentPage - 1) * coursesPerPage;
    const endIndex = startIndex + coursesPerPage;
    const currentCourses = filteredCourses.slice(startIndex, endIndex);

    if (currentCourses.length === 0) {
      coursesContainer.innerHTML = `
              <div class="no-courses">
                  <h3>No courses found</h3>
                  <p>Try adjusting your filters or search criteria</p>
              </div>
          `;
      return;
    }

    currentCourses.forEach((course) => {
      const courseCard = document.createElement("div");
      courseCard.className = "course-card";
      courseCard.innerHTML = `
              <div class="course-image">
                  <img src="${course.image}" alt="${course.title}">
                  <div class="course-duration">
                      <i class="far fa-clock"></i> ${course.duration}
                  </div>
              </div>
              <span class="course-level ${course.level.toLowerCase()}">${capitalizeFirstLetter(
        course.level
      )}</span>
              <div class="course-content">
                  <h3 class="course-title">${course.title}</h3>
                  <p class="course-description">${course.description}</p>
                  <div class="course-meta">
                      <div class="course-rating">
                          <i class="fas fa-star"></i>
                          <span>${course.rating}/7 Rating</span>
                      </div>
                      <div class="course-price">${course.price}</div>
                  </div>
                  <div class="course-stats">
                      <div class="course-lessons">
                          <i class="fas fa-book"></i> ${course.lessons} Lessons
                      </div>
                      <div class="course-students">
                          <i class="fas fa-users"></i> ${
                            course.students
                          } Students
                      </div>
                  </div>
                  <div class="course-action">
                      <a href="#">Enter Course</a>
                  </div>
              </div>
          `;
      coursesContainer.appendChild(courseCard);
    });
  }

  function applyFilters() {
    const categoryValue = categoryFilter.value;
    const levelValue = levelFilter.value;
    const durationValue = durationFilter.value;
    const searchValue = searchInput.value.toLowerCase().trim();

    filteredCourses = courses.filter((course) => {
      // Category filter
      if (categoryValue !== "all" && course.category !== categoryValue) {
        return false;
      }

      // Level filter
      if (levelValue !== "all" && course.level !== levelValue) {
        return false;
      }

      // Duration filter
      if (durationValue !== "all") {
        const weeks = parseInt(course.duration);
        if (durationValue === "short" && (weeks < 1 || weeks > 4)) {
          return false;
        } else if (durationValue === "medium" && (weeks < 5 || weeks > 8)) {
          return false;
        } else if (durationValue === "long" && weeks < 9) {
          return false;
        }
      }

      // Search filter
      if (
        searchValue &&
        !course.title.toLowerCase().includes(searchValue) &&
        !course.description.toLowerCase().includes(searchValue)
      ) {
        return false;
      }

      return true;
    });

    currentPage = 1;
    renderCourses();
    updatePagination();
  }

  function updatePagination() {
    const totalPages = Math.ceil(filteredCourses.length / coursesPerPage);

    // Update prev/next buttons
    prevPageBtn.disabled = currentPage === 1;
    nextPageBtn.disabled = currentPage === totalPages;

    // Update page numbers
    pageNumbers.innerHTML = "";

    // Determine which page numbers to show
    let startPage = Math.max(1, currentPage - 1);
    let endPage = Math.min(totalPages, startPage + 2);

    // Adjust if we're at the end
    if (endPage - startPage < 2) {
      startPage = Math.max(1, endPage - 2);
    }

    for (let i = startPage; i <= endPage; i++) {
      const span = document.createElement("span");
      span.textContent = i;
      if (i === currentPage) {
        span.className = "active";
      }
      span.addEventListener("click", function () {
        currentPage = i;
        renderCourses();
        updatePagination();
      });
      pageNumbers.appendChild(span);
    }
  }

  function capitalizeFirstLetter(string) {
    return string.charAt(0).toUpperCase() + string.slice(1);
  }
});
