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