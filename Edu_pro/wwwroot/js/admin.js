document.addEventListener('DOMContentLoaded', function () {
    // Select all checkbox functionality
    const selectAllCheckbox = document.getElementById('selectAll');
    const courseCheckboxes = document.querySelectorAll('.course-checkbox');

    selectAllCheckbox.addEventListener('change', function () {
        courseCheckboxes.forEach(checkbox => {
            checkbox.checked = selectAllCheckbox.checked;
        });
    });

    // Course search functionality
    const courseSearch = document.getElementById('courseSearch');
    courseSearch.addEventListener('input', function () {
        const searchTerm = courseSearch.value.toLowerCase();
        const courseRows = document.querySelectorAll('tbody tr');

        courseRows.forEach(row => {
            const courseTitle = row.querySelector('td:nth-child(4)').textContent.toLowerCase();
            if (courseTitle.includes(searchTerm)) {
                row.style.display = '';
            } else {
                row.style.display = 'none';
            }
        });
    });

    // Category filter functionality
    const categoryFilter = document.getElementById('categoryFilter');
    categoryFilter.addEventListener('change', function () {
        const selectedCategory = categoryFilter.value.toLowerCase();
        const courseRows = document.querySelectorAll('tbody tr');

        if (selectedCategory === '') {
            courseRows.forEach(row => {
                row.style.display = '';
            });
            return;
        }

        courseRows.forEach(row => {
            const category = row.querySelector('td:nth-child(5)').textContent.toLowerCase();
            if (category === selectedCategory) {
                row.style.display = '';
            } else {
                row.style.display = 'none';
            }
        });
    });

    // Status filter functionality
    const statusFilter = document.getElementById('statusFilter');
    statusFilter.addEventListener('change', function () {
        const selectedStatus = statusFilter.value.toLowerCase();
        const courseRows = document.querySelectorAll('tbody tr');

        if (selectedStatus === '') {
            courseRows.forEach(row => {
                row.style.display = '';
            });
            return;
        }

        courseRows.forEach(row => {
            const statusBadge = row.querySelector('td:nth-child(8) .badge');
            const status = statusBadge.textContent.toLowerCase();
            if (status === selectedStatus) {
                row.style.display = '';
            } else {
                row.style.display = 'none';
            }
        });
    });

    // Edit course functionality
    const editButtons = document.querySelectorAll('.edit-course');
    editButtons.forEach(button => {
        button.addEventListener('click', function () {
            const courseId = this.getAttribute('data-id');
            // In a real application, you would fetch the course data from the server
            // For this example, we'll use hardcoded data

            // Populate the edit form with course data based on the ID
            document.getElementById('editCourseId').value = courseId;

            if (courseId === '1') {
                document.getElementById('editCourseTitle').value = 'Web Development Fundamentals';
                document.getElementById('editCourseCategory').value = 'programming';
                document.getElementById('editCourseInstructor').value = '1';
                document.getElementById('editCoursePrice').value = '49.99';
                document.getElementById('editCourseDescription').value = 'A comprehensive course on web development basics.';
                document.getElementById('editCourseStatus').value = 'active';
                document.getElementById('editCourseDuration').value = '12.5';
                document.getElementById('editCourseLevel').value = 'beginner';
            } else if (courseId === '2') {
                document.getElementById('editCourseTitle').value = 'Graphic Design Masterclass';
                document.getElementById('editCourseCategory').value = 'design';
                document.getElementById('editCourseInstructor').value = '2';
                document.getElementById('editCoursePrice').value = '59.99';
                document.getElementById('editCourseDescription').value = 'Master the art of graphic design.';
                document.getElementById('editCourseStatus').value = 'active';
                document.getElementById('editCourseDuration').value = '15';
                document.getElementById('editCourseLevel').value = 'intermediate';
            } else if (courseId === '3') {
                document.getElementById('editCourseTitle').value = 'Digital Marketing Strategy';
                document.getElementById('editCourseCategory').value = 'marketing';
                document.getElementById('editCourseInstructor').value = '3';
                document.getElementById('editCoursePrice').value = '39.99';
                document.getElementById('editCourseDescription').value = 'Learn effective digital marketing strategies.';
                document.getElementById('editCourseStatus').value = 'draft';
                document.getElementById('editCourseDuration').value = '8';
                document.getElementById('editCourseLevel').value = 'all-levels';
            }
        });
    });

    // Delete course functionality
    const deleteButtons = document.querySelectorAll('.delete-course');
    deleteButtons.forEach(button => {
        button.addEventListener('click', function () {
            const courseId = this.getAttribute('data-id');
            document.getElementById('deleteCourseId').value = courseId;

            // Set the course title in the confirmation modal
            let courseTitle = '';
            if (courseId === '1') {
                courseTitle = 'Web Development Fundamentals';
            } else if (courseId === '2') {
                courseTitle = 'Graphic Design Masterclass';
            } else if (courseId === '3') {
                courseTitle = 'Digital Marketing Strategy';
            }
            document.getElementById('deleteCourseTitle').textContent = courseTitle;
        });
    });

    // Save new course
    document.getElementById('saveNewCourse').addEventListener('click', function () {
        const form = document.getElementById('addCourseForm');

        // Check if form is valid
        if (!form.checkValidity()) {
            form.reportValidity();
            return;
        }

        // In a real application, you would send the form data to the server
        alert('Course saved successfully!');

        // Close the modal
        const modal = bootstrap.Modal.getInstance(document.getElementById('addCourseModal'));
        modal.hide();

        // Reset the form
        form.reset();
    });

    // Update existing course
    document.getElementById('updateCourse').addEventListener('click', function () {
        const form = document.getElementById('editCourseForm');

        // Check if form is valid
        if (!form.checkValidity()) {
            form.reportValidity();
            return;
        }

        // In a real application, you would send the form data to the server
        alert('Course updated successfully!');

        // Close the modal
        const modal = bootstrap.Modal.getInstance(document.getElementById('editCourseModal'));
        modal.hide();
    });

    // Confirm delete
    document.getElementById('confirmDelete').addEventListener('click', function () {
        const courseId = document.getElementById('deleteCourseId').value;

        // In a real application, you would send a delete request to the server
        alert(`Course ID ${courseId} deleted successfully!`);

        // Close the modal
        const modal = bootstrap.Modal.getInstance(document.getElementById('deleteCourseModal'));
        modal.hide();

        // Remove the row from the table (in a real app, you might refresh the data)
        const row = document.querySelector(`.course-checkbox[data-id="${courseId}"]`).closest('tr');
        row.remove();
    });

    // Bulk delete
    document.getElementById('bulkDelete').addEventListener('click', function () {
        const selectedCourses = Array.from(document.querySelectorAll('.course-checkbox:checked'));

        if (selectedCourses.length === 0) {
            alert('Please select at least one course to delete.');
            return;
        }

        if (confirm(`Are you sure you want to delete ${selectedCourses.length} selected courses?`)) {
            // In a real application, you would send a delete request to the server
            const courseIds = selectedCourses.map(checkbox => checkbox.getAttribute('data-id'));
            alert(`Courses (IDs: ${courseIds.join(', ')}) deleted successfully!`);

            // Remove the rows from the table
            selectedCourses.forEach(checkbox => {
                const row = checkbox.closest('tr');
                row.remove();
            });

            // Uncheck the "Select All" checkbox
            document.getElementById('selectAll').checked = false;
        }
    });

    // Bulk activate
    document.getElementById('bulkActivate').addEventListener('click', function () {
        const selectedCourses = Array.from(document.querySelectorAll('.course-checkbox:checked'));

        if (selectedCourses.length === 0) {
            alert('Please select at least one course to activate.');
            return;
        }

        // In a real application, you would send a request to the server
        selectedCourses.forEach(checkbox => {
            const row = checkbox.closest('tr');
            const statusBadge = row.querySelector('td:nth-child(8) .badge');
            statusBadge.className = 'badge bg-success';
            statusBadge.textContent = 'Active';
        });

        alert(`${selectedCourses.length} courses set to Active status.`);
    });

    // Bulk draft
    document.getElementById('bulkDraft').addEventListener('click', function () {
        const selectedCourses = Array.from(document.querySelectorAll('.course-checkbox:checked'));

        if (selectedCourses.length === 0) {
            alert('Please select at least one course to set as draft.');
            return;
        }

        // In a real application, you would send a request to the server
        selectedCourses.forEach(checkbox => {
            const row = checkbox.closest('tr');
            const statusBadge = row.querySelector('td:nth-child(8) .badge');
            statusBadge.className = 'badge bg-warning text-dark';
            statusBadge.textContent = 'Draft';
        });

        alert(`${selectedCourses.length} courses set to Draft status.`);
    });
});