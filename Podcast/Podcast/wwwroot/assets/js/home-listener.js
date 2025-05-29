"use strict";
 
document.getElementById("toggle-icon").addEventListener("click", function () {
  const parametersDiv = document.querySelector("#parameters-filter .my-container .all .parameters");
  const podcastsDiv = document.querySelector(".podcasts");
  const allPodcastsDiv = document.querySelector(".podcasts .all-podcasts");

  if (parametersDiv.style.display !== "none") {
    parametersDiv.style.display = "none";
    podcastsDiv.style.gridColumnStart = "1";
    podcastsDiv.style.gridColumnEnd = "5";
    
    allPodcastsDiv.style.gridTemplateColumns = "1fr 1fr 1fr 1fr"; 
  } else {
    parametersDiv.style.display = "block";
    podcastsDiv.style.gridColumnStart = "2";
    podcastsDiv.style.gridColumnEnd = "5";
    
    allPodcastsDiv.style.gridTemplateColumns = "1fr 1fr 1fr"; 
  }
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