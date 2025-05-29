$(document).ready(function(){
  $('.podcasters').owlCarousel({
    loop: true,
    margin: 20,
    nav: false,
    dots: false,
    autoplay: true,
    autoplayTimeout: 3000,
    responsive: {
      0: { items: 1 },
      600: { items: 2 },
      1000: { items: 4 }
    }
  });
});


window.addEventListener("scroll", () => {
        let scrollPosition = window.scrollY;
      let header = document.querySelector("header");
       if (scrollPosition > 130) {
          header.style.position = "fixed";
           header.style.backgroundColor = "white";
          header.style.zIndex="1000";
           header.style.top = "0";
       } else {
          header.style.position = "relative";
     }
   });