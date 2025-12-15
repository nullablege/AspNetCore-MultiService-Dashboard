
document.addEventListener('DOMContentLoaded', () => {
    const navbarToggle = document.getElementById('navbarToggle');
    const navbarNav = document.getElementById('navbarNav');

    if (navbarToggle && navbarNav) {
        navbarToggle.addEventListener('click', () => {
            navbarNav.classList.toggle('active');
        });
    }

    const hotelCards = document.querySelectorAll('.hotel-card');

    hotelCards.forEach(card => {
        card.addEventListener('click', () => {
            const modalId = card.getAttribute('data-modal');
            const modal = document.getElementById(modalId);
            if (modal) {
                modal.classList.add('active');
                document.body.style.overflow = 'hidden';

                resetSlider(modal);
            }
        });
    });

    const modalCloseButtons = document.querySelectorAll('.modal-close');
    const modalOverlays = document.querySelectorAll('.modal-overlay');

    modalCloseButtons.forEach(btn => {
        btn.addEventListener('click', (e) => {
            e.stopPropagation();
            const modal = btn.closest('.modal-overlay');
            closeModal(modal);
        });
    });

    modalOverlays.forEach(overlay => {
        overlay.addEventListener('click', (e) => {
            if (e.target === overlay) {
                closeModal(overlay);
            }
        });
    });

    function closeModal(modal) {
        if (modal) {
            modal.classList.remove('active');
            document.body.style.overflow = '';
            resetSlider(modal);
        }
    }

    document.addEventListener('keydown', (e) => {
        if (e.key === 'Escape') {
            const activeModal = document.querySelector('.modal-overlay.active');
            if (activeModal) {
                closeModal(activeModal);
            }
        }
    });

    const sliders = document.querySelectorAll('.modal-slider');

    sliders.forEach(slider => {
        const images = slider.querySelectorAll('.slider-image');
        const dotsContainer = slider.querySelector('.slider-dots');
        const dots = slider.querySelectorAll('.slider-dot');
        const prevBtn = slider.querySelector('.slider-btn.prev');
        const nextBtn = slider.querySelector('.slider-btn.next');

        let currentIndex = 0;

        function showSlide(index) {
            if (index < 0) index = images.length - 1;
            if (index >= images.length) index = 0;

            images.forEach((img, i) => {
                img.classList.toggle('active', i === index);
            });
            dots.forEach((dot, i) => {
                dot.classList.toggle('active', i === index);
            });
            currentIndex = index;
        }

        if (prevBtn) {
            prevBtn.addEventListener('click', (e) => {
                e.stopPropagation();
                showSlide(currentIndex - 1);
            });
        }

        if (nextBtn) {
            nextBtn.addEventListener('click', (e) => {
                e.stopPropagation();
                showSlide(currentIndex + 1);
            });
        }

        dots.forEach((dot, i) => {
            dot.addEventListener('click', (e) => {
                e.stopPropagation();
                showSlide(i);
            });
        });
    });

    function resetSlider(modal) {
        const images = modal.querySelectorAll('.slider-image');
        const dots = modal.querySelectorAll('.slider-dot');

        images.forEach(img => img.classList.remove('active'));
        dots.forEach(dot => dot.classList.remove('active'));

        // Sadece ilk elemanları aktif yap
        if (images.length > 0) images[0].classList.add('active');
        if (dots.length > 0) dots[0].classList.add('active');
    }

    const searchForm = document.getElementById('searchForm');
    if (searchForm) {
        searchForm.addEventListener('submit', (e) => {

            const hotelsSection = document.getElementById('hotels');
            if (hotelsSection) {
                hotelsSection.scrollIntoView({ behavior: 'smooth' });
            }
        });
    }

    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
           
            const target = document.querySelector(this.getAttribute('href'));
            if (target) {
                target.scrollIntoView({ behavior: 'smooth' });
            }
            if (navbarNav) {
                navbarNav.classList.remove('active');
            }
        });
    });

    document.addEventListener('keydown', (e) => {
        const activeModal = document.querySelector('.modal-overlay.active');
        if (activeModal) {
            const slider = activeModal.querySelector('.modal-slider');
            const images = slider.querySelectorAll('.slider-image');
            const dots = slider.querySelectorAll('.slider-dot');

            let currentIndex = 0;
            images.forEach((img, i) => {
                if (img.classList.contains('active')) currentIndex = i;
            });

            if (e.key === 'ArrowLeft') {
                let newIndex = currentIndex - 1;
                if (newIndex < 0) newIndex = images.length - 1;
                images.forEach((img, i) => img.classList.toggle('active', i === newIndex));
                dots.forEach((dot, i) => dot.classList.toggle('active', i === newIndex));
            } else if (e.key === 'ArrowRight') {
                let newIndex = currentIndex + 1;
                if (newIndex >= images.length) newIndex = 0;
                images.forEach((img, i) => img.classList.toggle('active', i === newIndex));
                dots.forEach((dot, i) => dot.classList.toggle('active', i === newIndex));
            }
        }
    });
});
