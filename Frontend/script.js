// Check if user is logged in
if (!localStorage.getItem('isLoggedIn') && !window.location.href.includes('login.html') && !window.location.href.includes('signup.html')) {
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
  
  function setActiveView(viewId) {
    Object.values(views).forEach(v => { if(v) v.classList.remove('active-view'); });
    if(views[viewId]) views[viewId].classList.add('active-view');
    
    let titleText = "Dashboard";
    if(viewId === 'dashboard') {
      titleText = "Dashboard";
      loadCourses();
    }
    else if(viewId === 'rankings') titleText = "Round Rank";
    else if(viewId === 'lectures') titleText = "Lectures";
    else if(viewId === 'stats') titleText = "Round Stats";
    else if(viewId === 'progress') titleText = "My Progress";
    else if(viewId === 'tickets') titleText = "Tickets";
    else if(viewId === 'courses') titleText = "Enrolled Courses";
    else if(viewId === 'profile') titleText = "Profile";
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
  
  // Update Email
  document.getElementById('updateEmailBtn')?.addEventListener('click', () => {
    const newEmail = document.getElementById('newEmail')?.value.trim();
    const password = document.getElementById('emailPassword')?.value;
    if (!newEmail || !password) {
      alert('Please fill all fields');
      return;
    }
    if (!newEmail.includes('@')) {
      alert('Please enter a valid email address');
      return;
    }
    if (password === 'password123') {
      document.getElementById('currentEmail').innerText = newEmail;
      document.getElementById('newEmail').value = '';
      document.getElementById('emailPassword').value = '';
      showSuccess('Email updated successfully!');
    } else {
      alert('Incorrect password');
    }
  });
  
  // Update Password
  document.getElementById('updatePasswordBtn')?.addEventListener('click', () => {
    const currentPwd = document.getElementById('currentPassword')?.value;
    const newPwd = document.getElementById('newPassword')?.value;
    const confirmPwd = document.getElementById('confirmPassword')?.value;
    
    if (!currentPwd || !newPwd || !confirmPwd) {
      alert('Please fill all password fields');
      return;
    }
    if (newPwd !== confirmPwd) {
      alert('New passwords do not match');
      return;
    }
    if (newPwd.length < 8) {
      alert('Password must be at least 8 characters');
      return;
    }
    if (currentPwd === 'password123') {
      document.getElementById('currentPassword').value = '';
      document.getElementById('newPassword').value = '';
      document.getElementById('confirmPassword').value = '';
      showSuccess('Password changed successfully!');
    } else {
      alert('Current password is incorrect');
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
    const token = localStorage.getItem('token');
    const container = document.getElementById('dashboardCoursesContainer');
    
    if (!token) {
      if (!window.location.href.includes('login.html') && !window.location.href.includes('signup.html')) {
        window.location.href = 'login.html';
      }
      return;
    }

    try {
      const response = await fetch('http://justtech.runasp.net/api/courses', {
        headers: { 
          'Authorization': `Bearer ${token}`,
          'Accept': 'application/json'
        }
      });

      if (response.ok) {
        const courses = await response.json();
        if (!courses || courses.length === 0) {
          if (container) container.innerHTML = '<div class="no-courses" style="grid-column: 1/-1; text-align: center; padding: 40px; color: #64748b;">No courses available at the moment.</div>';
        } else {
          displayCourses(courses);
        }
      } else if (response.status === 401) {
        localStorage.removeItem('token');
        localStorage.removeItem('isLoggedIn');
        window.location.href = 'login.html';
      } else {
        if (container) container.innerHTML = '<div class="error-msg" style="grid-column: 1/-1; text-align: center; padding: 40px; color: #ef4444;">Failed to load courses. Please try again later.</div>';
      }
    } catch (error) {
      console.error('Error loading courses:', error);
      if (container) container.innerHTML = '<div class="error-msg" style="grid-column: 1/-1; text-align: center; padding: 40px; color: #ef4444;">Network error. Please check your connection.</div>';
    }
  }

  function displayCourses(courses) {
    const container = document.getElementById('dashboardCoursesContainer');
    if (!container) return;
    
    container.innerHTML = courses.map(course => `
      <div class="course-card">
        <div class="course-card-header">
          <h3>${escapeHtml(course.name)}</h3>
          <span class="course-badge">Course</span>
        </div>
        <p class="course-desc">${escapeHtml(course.description) || 'No description available'}</p>
        <div class="course-card-footer">
          <span class="course-plan"><i class="fas fa-calendar-alt"></i> ${escapeHtml(course.coursePlan) || 'Standard Plan'}</span>
          <button class="view-course-btn">View Details</button>
        </div>
      </div>
    `).join('');
  }

  function initTickets() { 
    if(document.getElementById('ticketsView')) renderTickets('all'); 
  }

  function initCourses() {
    if(document.getElementById('dashboardCoursesContainer')) {
      loadCourses();
    }
  }

  initTickets();
  initCourses();
  setActiveView('dashboard');
})();
