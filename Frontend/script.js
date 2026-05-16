// Check if user is logged in
console.log('Dashboard Init - Path:', window.location.pathname, 'Token:', !!localStorage.getItem('token'));
if (!localStorage.getItem('isLoggedIn') && !window.location.href.includes('login.html') && !window.location.href.includes('signup.html')) {
  console.log('No login session found, redirecting to login.html');
  window.location.href = 'login.html';
}

(function() {
  // view elements
  const views = {
    dashboard: document.getElementById('dashboardView'),
    rankings: document.getElementById('rankingsView'),
    lectures: document.getElementById('lecturesView'),
    stats: document.getElementById('statsView'),
    progress: document.getElementById('progressView'),
    tickets: document.getElementById('ticketsView'),
    courses: document.getElementById('coursesView'),
    profile: document.getElementById('profileView'),
    settings: document.getElementById('settingsView')
  };
  
  const navItems = document.querySelectorAll('.nav-item');
  const dynamicTitle = document.getElementById('dynamicPageTitle');
  const mobileToggle = document.getElementById('mobileToggle');
  const sidebar = document.getElementById('mainSidebar');
  
  async function ensureUserId() {
    let studentId = localStorage.getItem('userId');
    const token = localStorage.getItem('token');
    
    if (token && !studentId) {
      console.log("UserId missing but token found. Fetching userId...");
      try {
        const response = await fetch('http://justtech.runasp.net/api/students/me', {
          headers: { 'Authorization': `Bearer ${token}` }
        });
        if (response.ok) {
          const student = await response.json();
          localStorage.setItem('userId', student.id);
          console.log("UserId recovered:", student.id);
          return student.id;
        }
      } catch (err) {
        console.error("Failed to recover userId:", err);
      }
    }
    return studentId;
  }

  async function setActiveView(viewId) {
    // Ensure we have a userId before loading progress or enrollment views
    await ensureUserId();

    Object.values(views).forEach(v => { if(v) v.classList.remove('active-view'); });
    if(views[viewId]) views[viewId].classList.add('active-view');
    
    let titleText = "Home";
    if(viewId === 'dashboard') {
      titleText = "Home";
      loadCourses();
    }
    else if(viewId === 'rankings') titleText = "Round Rank";
    else if(viewId === 'lectures') titleText = "Lectures";
    else if(viewId === 'stats') titleText = "Round Stats";
    else if(viewId === 'progress') {
      titleText = "My Progress";
      loadStudentProgress(); // Explicitly load when switching to progress view
    }
    else if(viewId === 'tickets') titleText = "Tickets";
    else if(viewId === 'courses') {
      titleText = "Enrolled Courses";
      loadUserEnrollments();
    }
    else if(viewId === 'profile') {
      titleText = "Profile";
      loadStudentProfile();
    }
    else if(viewId === 'settings') titleText = "Settings";
    if(dynamicTitle) dynamicTitle.innerText = titleText;
    
    navItems.forEach(item => {
      item.classList.remove('active');
      const navVal = item.getAttribute('data-nav');
      if(navVal === viewId) item.classList.add('active');
    });
    
    if(window.innerWidth <= 880 && sidebar) {
      sidebar.classList.remove('mobile-open');
    }
  }
  
  // Navigation event listeners
  document.getElementById('navDashboard')?.addEventListener('click', (e) => { e.preventDefault(); setActiveView('dashboard'); });
  document.getElementById('navRankings')?.addEventListener('click', (e) => { e.preventDefault(); setActiveView('rankings'); });
  document.getElementById('navLectures')?.addEventListener('click', (e) => { e.preventDefault(); setActiveView('lectures'); });
  document.getElementById('navStats')?.addEventListener('click', (e) => { e.preventDefault(); setActiveView('stats'); });
  document.getElementById('navProgress')?.addEventListener('click', (e) => { e.preventDefault(); setActiveView('progress'); });
  document.getElementById('navTickets')?.addEventListener('click', (e) => { e.preventDefault(); setActiveView('tickets'); renderTickets(); });
  document.getElementById('navCourses')?.addEventListener('click', (e) => { e.preventDefault(); setActiveView('courses'); });
  document.getElementById('navProfile')?.addEventListener('click', (e) => { e.preventDefault(); setActiveView('profile'); });
  
  // Settings navigation from sidebar footer
  const settingsNav = document.getElementById('settingsNav');
  if(settingsNav) {
    settingsNav.addEventListener('click', (e) => {
      e.preventDefault();
      setActiveView('settings');
    });
  }
  
  // Mobile menu toggle
  if(mobileToggle) {
    mobileToggle.addEventListener('click', function(e) {
      e.stopPropagation();
      if(sidebar) sidebar.classList.toggle('mobile-open');
    });
  }
  
  document.addEventListener('click', function(e) {
    if(window.innerWidth <= 880 && sidebar && mobileToggle) {
      if(!sidebar.contains(e.target) && !mobileToggle.contains(e.target)) {
        sidebar.classList.remove('mobile-open');
      }
    }
  });
  
  // Avatar dropdown
  const avatarBtn = document.querySelector('.avatar-wrapper .avatar-small');
  const avatarDropdown = document.getElementById('avatarDropdown');
  
  if (avatarBtn && avatarDropdown) {
    avatarBtn.addEventListener('click', function(e) {
      e.stopPropagation();
      avatarDropdown.classList.toggle('show');
    });
  
    document.addEventListener('click', function() {
      if(avatarDropdown) avatarDropdown.classList.remove('show');
    });
  
    avatarDropdown.addEventListener('click', function(e) {
      e.stopPropagation();
    });
  }
  
  // Settings button from avatar dropdown
  const settingsBtn = document.getElementById('settingsBtn');
  if (settingsBtn) {
    settingsBtn.addEventListener('click', function(e) {
      e.preventDefault();
      setActiveView('settings');
      if(avatarDropdown) avatarDropdown.classList.remove('show');
    });
  }
  
  // Logout button from avatar dropdown
  const logoutBtn = document.getElementById('logoutBtn');
  if (logoutBtn) {
    logoutBtn.addEventListener('click', function(e) {
      e.preventDefault();
      localStorage.removeItem('isLoggedIn');
      window.location.href = 'login.html';
    });
  }
  
  // TICKETS LOGIC
  let ticketsArray = [];
  const ticketsListDiv = document.getElementById('ticketsDynamicList');
  const noTicketsMsgDiv = document.getElementById('noTicketsMsg');
  
  function renderTickets(filter = 'all') {
    if(!ticketsListDiv) return;
    let filtered = ticketsArray;
    if(filter === 'pending') filtered = ticketsArray.filter(t => t.status === 'pending');
    else if(filter === 'open') filtered = ticketsArray.filter(t => t.status === 'open');
    else if(filter === 'in-progress') filtered = ticketsArray.filter(t => t.status === 'in-progress');
    else if(filter === 'closed') filtered = ticketsArray.filter(t => t.status === 'closed');
    
    if(filtered.length === 0) { 
      if(noTicketsMsgDiv) noTicketsMsgDiv.style.display = 'flex'; 
      ticketsListDiv.style.display = 'none'; 
    } else { 
      if(noTicketsMsgDiv) noTicketsMsgDiv.style.display = 'none'; 
      ticketsListDiv.style.display = 'grid'; 
      ticketsListDiv.innerHTML = filtered.map(ticket => `
        <div class="ticket-item-card status-${ticket.status}">
          <div class="ticket-header"><strong>${escapeHtml(ticket.subject)}</strong><span class="ticket-status-badge">${ticket.status.toUpperCase()}</span></div>
          <div class="ticket-desc">${escapeHtml(ticket.description)}</div>
          <div class="ticket-footer"><small>${new Date(ticket.date).toLocaleString()}</small><button class="delete-ticket-btn" data-id="${ticket.id}"><i class="fas fa-trash-alt"></i></button></div>
        </div>`).join('');
      
      document.querySelectorAll('.delete-ticket-btn').forEach(btn => { 
        btn.addEventListener('click', (e) => { 
          const id = parseInt(btn.getAttribute('data-id')); 
          ticketsArray = ticketsArray.filter(t => t.id !== id); 
          renderTickets(getActiveFilter()); 
        }); 
      });
    }
  }
  
  function getActiveFilter() { 
    const active = document.querySelector('.filter-btn.active-filter'); 
    return active ? active.getAttribute('data-filter') : 'all'; 
  }
  
  function escapeHtml(str) { 
    if(!str) return ''; 
    return str.replace(/[&<>]/g, function(m) { 
      if(m === '&') return '&amp;'; 
      if(m === '<') return '&lt;'; 
      if(m === '>') return '&gt;'; 
      return m;
    }); 
  }
  
  window.renderTickets = renderTickets;
  
  const sendBtn = document.getElementById('sendTicketBtn');
  if(sendBtn) {
    sendBtn.addEventListener('click', () => {
      const subject = document.getElementById('ticketSubject')?.value.trim();
      const desc = document.getElementById('ticketDesc')?.value.trim();
      if(!subject || !desc) { alert("Please fill both subject and description"); return; }
      const newTicket = { id: Date.now(), subject, description: desc, status: 'open', date: new Date().toISOString() };
      ticketsArray.unshift(newTicket);
      const subjectInput = document.getElementById('ticketSubject');
      const descInput = document.getElementById('ticketDesc');
      if(subjectInput) subjectInput.value = '';
      if(descInput) descInput.value = '';
      renderTickets(getActiveFilter());
      const filterBtns = document.querySelectorAll('.filter-btn');
      filterBtns.forEach(btn => { btn.classList.remove('active-filter'); if(btn.getAttribute('data-filter') === 'all') btn.classList.add('active-filter'); });
    });
  }
  
  document.querySelectorAll('.filter-btn').forEach(btn => { 
    btn.addEventListener('click', function() { 
      document.querySelectorAll('.filter-btn').forEach(b => b.classList.remove('active-filter')); 
      this.classList.add('active-filter'); 
      renderTickets(this.getAttribute('data-filter')); 
    }); 
  });
  
  // SETTINGS PAGE FUNCTIONS
  function showSuccess(message) {
    let successDiv = document.querySelector('.success-message');
    if (!successDiv) {
      successDiv = document.createElement('div');
      successDiv.className = 'success-message';
      document.body.appendChild(successDiv);
    }
    successDiv.innerHTML = `<i class="fas fa-check-circle"></i> ${message}`;
    successDiv.style.display = 'block';
    setTimeout(() => {
      successDiv.style.display = 'none';
    }, 3000);
  }
  
  // Update Password
  document.getElementById('updatePasswordBtn')?.addEventListener('click', async () => {
    const currentPwd = document.getElementById('currentPassword')?.value;
    const newPwd = document.getElementById('newPassword')?.value;
    const confirmPwd = document.getElementById('confirmPassword')?.value;
    const token = localStorage.getItem('token');
    const updateBtn = document.getElementById('updatePasswordBtn');
    
    if (!currentPwd || !newPwd || !confirmPwd) {
      alert('Please fill all password fields');
      return;
    }
    if (newPwd !== confirmPwd) {
      alert('New passwords do not match');
      return;
    }
    if (newPwd.length < 6) {
      alert('Password must be at least 6 characters');
      return;
    }

    if (!token) {
      alert('Session expired. Please login again.');
      window.location.href = 'login.html';
      return;
    }

    updateBtn.disabled = true;
    updateBtn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Updating...';

    try {
      const response = await fetch('http://justtech.runasp.net/api/auth/change-password', {
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json',
          'Accept': 'application/json'
        },
        body: JSON.stringify({
          currentPassword: currentPwd,
          newPassword: newPwd,
          confirmNewPassword: confirmPwd
        })
      });

      if (response.ok) {
        showSuccess('Password changed successfully!');
        document.getElementById('currentPassword').value = '';
        document.getElementById('newPassword').value = '';
        document.getElementById('confirmPassword').value = '';
      } else {
        const errorData = await response.json().catch(() => ({}));
        alert(errorData.message || 'Failed to change password. Please check your current password.');
      }
    } catch (error) {
      console.error('Password change error:', error);
      alert('Network error. Please try again.');
    } finally {
      updateBtn.disabled = false;
      updateBtn.innerHTML = '<i class="fas fa-key"></i> Update Password';
    }
  });
  
  // Logout from settings
  document.getElementById('logoutSettingsBtn')?.addEventListener('click', () => {
    localStorage.removeItem('isLoggedIn');
    window.location.href = 'login.html';
  });
  
  // Delete Account modal
  const deleteModal = document.getElementById('deleteModal');
  document.getElementById('deleteAccountBtn')?.addEventListener('click', () => {
    if (deleteModal) deleteModal.style.display = 'flex';
  });
  
  document.getElementById('cancelDeleteBtn')?.addEventListener('click', () => {
    if (deleteModal) deleteModal.style.display = 'none';
  });
  
  document.getElementById('confirmDeleteBtn')?.addEventListener('click', () => {
    localStorage.removeItem('isLoggedIn');
    alert('Account deleted successfully');
    window.location.href = 'signup.html';
  });
  
  window.addEventListener('click', (e) => {
    if (deleteModal && e.target === deleteModal) {
      deleteModal.style.display = 'none';
    }
  });
  
  async function loadCourses() {
    console.log('loadCourses called - timestamp:', new Date());
    const token = localStorage.getItem('token');
    const studentId = localStorage.getItem('userId');
    const container = document.getElementById('dashboardCoursesContainer');
    
    console.log('Token exists:', !!token);
    console.log('Student ID:', studentId);

    if (!container) return;

    // 1. Check Cache First
    const cachedCourses = sessionStorage.getItem('cached_courses');
    const cachedEnrollments = sessionStorage.getItem('cached_enrollment_names');
    
    if (cachedCourses && cachedEnrollments) {
      console.log("Loading courses from cache...");
      displayCourses(JSON.parse(cachedCourses), JSON.parse(cachedEnrollments), true);
    } else {
      // 2. Show Skeleton UI if no cache
      showSkeletonUI(container);
    }

    try {
      const [coursesRes, enrollmentsRes] = await Promise.all([
        fetch('http://justtech.runasp.net/api/courses', {
          headers: { 'Authorization': `Bearer ${token}`, 'Accept': 'application/json' }
        }),
        fetch(`http://justtech.runasp.net/api/enrollments/student/${studentId}`, {
          headers: { 'Authorization': `Bearer ${token}`, 'Accept': 'application/json' }
        }).catch(() => ({ ok: false }))
      ]);

      if (coursesRes.ok) {
        const courses = await coursesRes.json();
        let enrolledCourseNames = [];
        if (enrollmentsRes.ok) {
          const enrollments = await enrollmentsRes.json();
          enrolledCourseNames = enrollments.map(enr => enr.courseName);
        }

        // 3. Update Cache
        sessionStorage.setItem('cached_courses', JSON.stringify(courses));
        sessionStorage.setItem('cached_enrollment_names', JSON.stringify(enrolledCourseNames));

        // 4. Render Fresh Data (chunked)
        if (!courses || courses.length === 0) {
          container.innerHTML = '<div class="no-courses" style="grid-column: 1/-1; text-align: center; padding: 40px; color: #64748b;">No courses available at the moment.</div>';
        } else {
          displayCourses(courses, enrolledCourseNames, false);
        }
      } else if (coursesRes.status === 401) {
        localStorage.removeItem('token');
        localStorage.removeItem('isLoggedIn');
        window.location.href = 'login.html';
      }
    } catch (error) {
      console.error('Error loading courses:', error);
      if (container) {
        container.innerHTML = `
          <div class="error-msg" style="grid-column: 1/-1; text-align: center; padding: 40px; color: #ef4444;">
            <i class="fas fa-exclamation-circle" style="font-size: 2rem; margin-bottom: 10px; display: block;"></i>
            <p>Failed to load courses. Please check your connection and try again.</p>
            <button onclick="location.reload()" class="outline-btn" style="margin-top: 15px;">Retry</button>
          </div>`;
      }
    }
  }

  function showSkeletonUI(container) {
    container.innerHTML = Array(4).fill(0).map(() => `
      <div class="skeleton-card">
        <div class="skeleton-title"></div>
        <div class="skeleton-text"></div>
        <div class="skeleton-text" style="width: 85%"></div>
        <div class="skeleton-footer">
          <div class="skeleton-btn"></div>
          <div class="skeleton-btn"></div>
        </div>
      </div>
    `).join('');
  }

  function displayCourses(courses, enrolledCourseNames, isFromCache) {
    const container = document.getElementById('dashboardCoursesContainer');
    if (!container) return;

    // If it's a background update and data is identical, skip re-render
    if (!isFromCache) {
      const currentCards = container.querySelectorAll('.course-card');
      if (currentCards.length === courses.length && !container.querySelector('.skeleton-card')) {
        // Simple heuristic: if counts match and it's already rendered, skip
        return; 
      }
      container.innerHTML = ''; 
    } else {
      container.innerHTML = '';
    }

    // 2. Chunk the Rendering (Batching)
    const CHUNK_SIZE = 4;
    let index = 0;

    function renderNextBatch() {
      const batch = courses.slice(index, index + CHUNK_SIZE);
      const html = batch.map(course => {
        const isEnrolled = enrolledCourseNames.includes(course.name);
        return `
          <div class="course-card" data-id="${course.id}">
            <div class="course-card-header">
              <h3>${escapeHtml(course.name)}</h3>
              <span class="course-badge">Course</span>
            </div>
            <p class="course-desc">${escapeHtml(course.description) || 'No description available'}</p>
            <div class="course-card-footer">
              <span class="course-plan"><i class="fas fa-calendar-alt"></i> ${escapeHtml(course.coursePlan) || 'Standard Plan'}</span>
              <div class="course-actions-wrap">
                <button class="view-course-btn outline-btn">View Details</button>
                ${isEnrolled 
                  ? `<button class="enroll-course-btn enrolled-btn" disabled>Enrolled</button>`
                  : `<button class="enroll-course-btn solid-btn" data-course-id="${course.id}">Enroll Now</button>`
                }
              </div>
            </div>
          </div>
        `;
      }).join('');

      container.insertAdjacentHTML('beforeend', html);
      
      // Re-attach event listeners for the new batch
      const newCards = container.querySelectorAll(`.course-card:nth-last-child(-n+${batch.length})`);
      newCards.forEach(card => {
        const enrollBtn = card.querySelector('.enroll-course-btn.solid-btn');
        if (enrollBtn) {
          enrollBtn.addEventListener('click', function(e) {
            e.stopPropagation();
            enrollCourse(this.getAttribute('data-course-id'), this);
          });
        }
        card.querySelector('.view-course-btn').addEventListener('click', function() {
          showCourseDetails(card.getAttribute('data-id'), enrolledCourseNames);
        });
      });

      index += CHUNK_SIZE;
      if (index < courses.length) {
        // Use timeout to let the UI breathe
        setTimeout(renderNextBatch, 0);
      }
    }

    renderNextBatch();
  }

  async function showCourseDetails(courseId, enrolledCourseNames) {
    const token = localStorage.getItem('token');
    const modal = document.getElementById('courseDetailsModal');
    if (!token || !modal) return;

    try {
      const response = await fetch(`http://justtech.runasp.net/api/courses/${courseId}`, {
        headers: { 'Authorization': `Bearer ${token}` }
      });

      if (response.ok) {
        const course = await response.json();
        const isEnrolled = enrolledCourseNames.includes(course.name);

        document.getElementById('modalCourseName').innerText = course.name;
        document.getElementById('modalCourseDesc').innerText = course.description || 'No description available.';
        document.getElementById('modalCoursePlan').innerText = course.coursePlan || 'Standard Plan';
        
        const footer = document.getElementById('modalCourseFooter');
        if (isEnrolled) {
          footer.innerHTML = `<button class="enroll-course-btn enrolled-btn" disabled>Enrolled</button>`;
        } else {
          footer.innerHTML = `<button class="enroll-course-btn solid-btn modal-enroll-btn" data-course-id="${course.id}">Enroll Now</button>`;
          footer.querySelector('.modal-enroll-btn').addEventListener('click', function() {
            enrollCourse(course.id, this);
          });
        }

        modal.style.display = 'flex';
      }
    } catch (error) {
      console.error('Error fetching course details:', error);
    }
  }

  // Close modal logic
  document.getElementById('closeCourseModal')?.addEventListener('click', () => {
    document.getElementById('courseDetailsModal').style.display = 'none';
  });

  window.addEventListener('click', (e) => {
    const modal = document.getElementById('courseDetailsModal');
    if (e.target === modal) {
      modal.style.display = 'none';
    }
  });

  async function enrollCourse(courseId, button) {
    const token = localStorage.getItem('token');
    const studentId = localStorage.getItem('userId');

    if (!token || !studentId) {
      alert('Session expired. Please login again.');
      window.location.href = 'login.html';
      return;
    }

    const originalText = button.innerText;
    button.disabled = true;
    button.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Enrolling...';

    try {
      const response = await fetch('http://justtech.runasp.net/api/enrollments', {
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json',
          'Accept': 'application/json'
        },
        body: JSON.stringify({
          studentId: studentId,
          roundId: 1 // Fixed as requested
        })
      });

      if (response.ok) {
        showSuccess('Successfully enrolled in the course!');
        button.innerText = 'Enrolled';
        button.classList.remove('solid-btn');
        button.classList.add('enrolled-btn');
        button.disabled = true;
      } else {
        const errorData = await response.json().catch(() => ({}));
        alert(errorData.message || 'Enrollment failed. Please try again.');
        button.disabled = false;
        button.innerText = originalText;
      }
    } catch (error) {
      console.error('Enrollment error:', error);
      alert('Network error. Please try again.');
      button.disabled = false;
      button.innerText = originalText;
    }
  }

  async function loadUserEnrollments() {
    const token = localStorage.getItem('token');
    const studentId = localStorage.getItem('userId');
    const container = document.getElementById('coursesContainer');
    
    if (!token || !studentId) return;

    try {
      const response = await fetch(`http://justtech.runasp.net/api/enrollments/student/${studentId}`, {
        headers: { 
          'Authorization': `Bearer ${token}`,
          'Accept': 'application/json'
        }
      });

      if (response.ok) {
        const enrollments = await response.json();
        displayUserEnrollments(enrollments);
      } else {
        if (container) container.innerHTML = '<div class="error-msg" style="grid-column: 1/-1; text-align: center; padding: 40px; color: #ef4444;">Failed to load your enrollments.</div>';
      }
    } catch (error) {
      console.error('Error loading enrollments:', error);
      if (container) container.innerHTML = '<div class="error-msg" style="grid-column: 1/-1; text-align: center; padding: 40px; color: #ef4444;">Network error while fetching enrollments.</div>';
    }
  }

  function displayUserEnrollments(enrollments) {
    const container = document.getElementById('coursesContainer');
    if (!container) return;
    
    if (!enrollments || enrollments.length === 0) {
      container.innerHTML = '<div class="no-courses" style="grid-column: 1/-1; text-align: center; padding: 40px; color: #64748b;">You are not enrolled in any courses yet.</div>';
      return;
    }

    container.innerHTML = enrollments.map(enr => `
      <div class="course-master-card">
        <div class="course-badge-top">
          <span class="course-status active-status">${escapeHtml(enr.status)}</span>
          <span class="course-mode"><i class="fas fa-clock"></i> Enrolled on ${new Date(enr.enrolledAt).toLocaleDateString()}</span>
        </div>
        <div class="course-title-section">
          <h3>${escapeHtml(enr.courseName)}</h3>
          <p class="course-sub">${escapeHtml(enr.roundName)}</p>
        </div>
        <div class="course-stats-mini">
          <div class="course-stat-item">
            <span class="stat-label">Enrollment Date</span>
            <span class="stat-number" style="font-size: 1rem;">${new Date(enr.enrolledAt).toLocaleDateString()}</span>
          </div>
        </div>
        <div class="course-actions">
          <button class="course-btn solid-btn enter-classroom-btn" data-round-id="${enr.roundId}">Enter Classroom</button>
        </div>
      </div>
    `).join('');

    // Attach event listeners for Classroom button
    document.querySelectorAll('.enter-classroom-btn').forEach(btn => {
      btn.addEventListener('click', function() {
        const roundId = this.getAttribute('data-round-id');
        window.location.href = `course-content.html?roundId=${roundId}`;
      });
    });
  }

  async function loadStudentProfile() {
    const token = localStorage.getItem('token');
    const studentId = localStorage.getItem('userId');
    
    if (!token || !studentId) return;

    try {
      const response = await fetch(`http://justtech.runasp.net/api/students/${studentId}`, {
        headers: { 'Authorization': `Bearer ${token}` }
      });

      if (response.ok) {
        const student = await response.json();
        // Fill form fields
        document.getElementById('profileName').value = student.name || '';
        document.getElementById('profileEmail').value = student.email || '';
        document.getElementById('profilePhone').value = student.phoneNumber || '';
        document.getElementById('profileBirthdate').value = student.birthDate ? student.birthDate.split('T')[0] : '';
        document.getElementById('profileCountry').value = student.country || '';
        document.getElementById('profileCity').value = student.city || '';
        document.getElementById('profileCollege').value = student.college || '';
        document.getElementById('profileProfession').value = student.profession || '';
        
        // Fill display fields
        document.getElementById('profileDisplayName').innerText = student.name || 'Student';
        document.getElementById('profileDisplayEmail').innerText = student.email || '';
      }
    } catch (error) {
      console.error('Error loading profile:', error);
    }
  }

  async function updateStudentProfile() {
    const token = localStorage.getItem('token');
    const studentId = localStorage.getItem('userId');
    const saveBtn = document.getElementById('saveProfileBtn');
    
    if (!token || !studentId) return;

    const studentData = {
      name: document.getElementById('profileName').value,
      phoneNumber: document.getElementById('profilePhone').value,
      birthDate: document.getElementById('profileBirthdate').value,
      country: document.getElementById('profileCountry').value,
      city: document.getElementById('profileCity').value,
      college: document.getElementById('profileCollege').value,
      profession: document.getElementById('profileProfession').value
    };

    saveBtn.disabled = true;
    saveBtn.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Saving...';

    try {
      const response = await fetch(`http://justtech.runasp.net/api/students/${studentId}`, {
        method: 'PUT',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(studentData)
      });

      if (response.ok) {
        showSuccess('Profile updated successfully');
        loadStudentProfile(); // Refresh display
      } else {
        const error = await response.json().catch(() => ({}));
        alert(error.message || 'Failed to update profile');
      }
    } catch (error) {
      console.error('Error updating profile:', error);
      alert('Network error while updating profile');
    } finally {
      saveBtn.disabled = false;
      saveBtn.innerHTML = '<i class="fas fa-save"></i> Save Changes';
    }
  }

  document.getElementById('saveProfileBtn')?.addEventListener('click', updateStudentProfile);

  function initTickets() { 
    if(document.getElementById('ticketsView')) renderTickets('all'); 
  }

  async function loadStudentProgress() {
    const token = localStorage.getItem('token');
    // Re-check ID just in case
    const studentId = await ensureUserId();
    const listContainer = document.getElementById('progressListContainer');
    
    if (!token || !studentId || !listContainer) return;

    console.log("loadStudentProgress starting for studentId:", studentId);
    try {
      const response = await fetch(`http://justtech.runasp.net/api/enrollments/student/${studentId}`, {
        headers: { 'Authorization': `Bearer ${token}` }
      });

      console.log("Enrollment response status:", response.status);

      if (response.ok) {
        const enrollments = await response.json();
        
        if (!enrollments || enrollments.length === 0) {
          listContainer.innerHTML = '<div class="no-courses" style="grid-column: 1/-1; text-align: center; padding: 40px; color: #64748b;">No courses enrolled yet. Browse courses to start learning.</div>';
          document.getElementById('totalEnrolledCount').innerText = '0';
          document.getElementById('overallAvgValue').innerText = '0%';
          document.getElementById('overallAvgBar').style.width = '0%';
          return;
        }

        console.log(`Found ${enrollments.length} enrollments. Fetching percentages...`);
        document.getElementById('totalEnrolledCount').innerText = enrollments.length;
        document.getElementById('studentJoinDate').innerText = new Date(enrollments[0].enrolledAt).toLocaleDateString();

        const progressPromises = enrollments.map(async (enr) => {
          try {
            const controller = new AbortController();
            const id = setTimeout(() => controller.abort(), 10000); // 10s timeout
            
            console.log(`Fetching percentage for round ${enr.roundId}...`);
            const res = await fetch(`http://justtech.runasp.net/api/progress/student/${studentId}/round/${enr.roundId}/percentage`, {
              headers: { 'Authorization': `Bearer ${token}` },
              signal: controller.signal
            });
            clearTimeout(id);
            
            const data = res.ok ? await res.json() : 0;
            // Handle if API returns { progressPercentage: X } or just X
            const percentage = typeof data === 'object' ? data.progressPercentage : data;
            console.log(`Round ${enr.roundId} backend percentage:`, percentage);
            return { ...enr, percentage: percentage || 0 };
          } catch (e) {
            console.error(`Could not fetch percentage for round ${enr.roundId}:`, e);
            return { ...enr, percentage: 0 };
          }
        });

        const detailedEnrollments = await Promise.all(progressPromises);
        
        const totalPercentage = detailedEnrollments.reduce((sum, enr) => sum + enr.percentage, 0);
        const avgPercentage = detailedEnrollments.length > 0 ? Math.round(totalPercentage / detailedEnrollments.length) : 0;
        console.log(`Average calculated: ${avgPercentage}% based on ${detailedEnrollments.length} courses.`);

        const avgValEl = document.getElementById('overallAvgValue');
        const avgBarEl = document.getElementById('overallAvgBar');
        
        if (avgValEl) avgValEl.innerText = `${avgPercentage}%`;
        if (avgBarEl) avgBarEl.style.width = `${avgPercentage}%`;

        listContainer.innerHTML = detailedEnrollments.map(enr => {
          console.log(`Rendering ${enr.courseName}: ${enr.percentage}%`);
          return `
            <div class="course-master-card">
              <div class="course-badge-top">
                <span class="course-status active-status">In Progress</span>
                <span class="course-mode"><i class="fas fa-calendar"></i> Enrolled: ${new Date(enr.enrolledAt).toLocaleDateString()}</span>
              </div>
              <div class="course-title-section">
                <h3>${escapeHtml(enr.courseName)}</h3>
                <p class="course-sub">${escapeHtml(enr.roundName)}</p>
              </div>
              <div class="progress-individual" style="padding: 0;">
                <div class="progress-metric">
                  <div class="metric-label">Course Completion</div>
                  <div class="metric-value" style="font-size: 1.2rem;">${enr.percentage}%</div>
                  <div class="progress-bar-global">
                    <div class="global-fill" style="width:${enr.percentage}%"></div>
                  </div>
                </div>
              </div>
            </div>
          `;
        }).join('');
      } else {
        listContainer.innerHTML = '<div class="error-msg" style="grid-column: 1/-1; text-align: center; padding: 40px; color: #ef4444;">Could not fetch your enrollments. Please check your connection.</div>';
      }
    } catch (error) {
      console.error('Error loading progress:', error);
      listContainer.innerHTML = '<div class="error-msg" style="grid-column: 1/-1; text-align: center; padding: 40px; color: #ef4444;">Network error while loading progress.</div>';
    }
  }

  async function init() {
    try {
      console.log('Initializing application...');
      await ensureUserId();
      initTickets();
      // initCourses(); // Removed as it was non-existent and causing crashes
      loadStudentProgress();
      
      const urlParams = new URLSearchParams(window.location.search);
      const initialView = urlParams.get('view') || 'dashboard';
      console.log('Setting initial view:', initialView);
      setActiveView(initialView);
    } catch (err) {
      console.error('Initialization failed:', err);
    }
  }

  init();
})();
