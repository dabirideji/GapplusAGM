//FUNCTION FOR GETTING USER DATA//
let baseUri = "http://localhost:5069/api";

console.log("utility function connected");

function getUserData() {
  var UserDataString = sessionStorage.getItem("UserDataString");
  if (UserDataString == null) {
    window.location.href = "./UserLogin.html";
  }
  // console.log(UserDataString)
  var userData = JSON.parse(UserDataString);
  // console.log(userData)
  return userData;
}

function tryGetQueryValue(key) {
  var queryParams = new URLSearchParams(window.location.search);
  if (queryParams.has(key)) {
    return queryParams.get(key);
  }
  return false;
}

function ApiConsumeSettings(method = "GET", object) {
  return {
    headers: {
      "Content-Type": "application/json",
    },
    method: method.toLocaleUpperCase(),
    body: JSON.stringify(object),
  };
}

//A FETCH HELPER FOR EASY API CONSUMPTION
function consume(Endpoint, settings = null, ...parameters) {
  var url = baseUri + "/" + Endpoint;
  parameters.forEach((x) => {
    if (x !== null && x.trim() !== "") {
      url += "/" + x;
    }
  });

  return fetch(url, settings).then((x) => {
    return x.json();
  });
}

function formatDate(dateFromApi) {
  var date = new Date(dateFromApi);
  return date.toLocaleTimeString();
}

////////////////// ACTIVITY INDICATOR SECTION /////////////////
function showActivityIndicatorForPage(elementId = null, duration = 6) {
  //SPECIFY THE DURATION OF THE INDICATOR IN SECONDS.
  if (elementId != null) {
    let body = document.querySelector(`#${elementId}`);
    //   body.innerHTML += `
    //     <div role="status" id="activityIndicator"
    //     class="w-screen bg-gray-300 bg-opacity-80 z-50 float fixed top-0 right-0 h-screen flex items-center justify-around">
    //     <!-- <svg aria-hidden="true" class="w-8 h-[60vh] text-24 text-gray-200 animate-spin dark:text-gray-600 fill-primary" -->

    //     <svg aria-hidden="true"
    //         class="mx-auto w-[15vw] h-[20vh] text-24 text-gray-200 animate-spin dark:text-gray-600 fill-primary"
    //         viewBox="0 0 100 101" fill="none" xmlns="http://www.w3.org/2000/svg">
    //         <path
    //             d="M100 50.5908C100 78.2051 77.6142 100.591 50 100.591C22.3858 100.591 0 78.2051 0 50.5908C0 22.9766 22.3858 0.59082 50 0.59082C77.6142 0.59082 100 22.9766 100 50.5908ZM9.08144 50.5908C9.08144 73.1895 27.4013 91.5094 50 91.5094C72.5987 91.5094 90.9186 73.1895 90.9186 50.5908C90.9186 27.9921 72.5987 9.67226 50 9.67226C27.4013 9.67226 9.08144 27.9921 9.08144 50.5908Z"
    //             fill="currentColor" />
    //         <path
    //             d="M93.9676 39.0409C96.393 38.4038 97.8624 35.9116 97.0079 33.5539C95.2932 28.8227 92.871 24.3692 89.8167 20.348C85.8452 15.1192 80.8826 10.7238 75.2124 7.41289C69.5422 4.10194 63.2754 1.94025 56.7698 1.05124C51.7666 0.367541 46.6976 0.446843 41.7345 1.27873C39.2613 1.69328 37.813 4.19778 38.4501 6.62326C39.0873 9.04874 41.5694 10.4717 44.0505 10.1071C47.8511 9.54855 51.7191 9.52689 55.5402 10.0491C60.8642 10.7766 65.9928 12.5457 70.6331 15.2552C75.2735 17.9648 79.3347 21.5619 82.5849 25.841C84.9175 28.9121 86.7997 32.2913 88.1811 35.8758C89.083 38.2158 91.5421 39.6781 93.9676 39.0409Z"
    //             fill="currentFill" />
    //     </svg>
    // </div>
    //     `;

    body.innerHTML += `
      <div role="status" id="activityIndicator"
      class=" bg-gray-300 bg-opacity-80 z-50 w-full h-full absolute top-0 right-0 flex items-center justify-around">
  
  
      <svg aria-hidden="true"
          class="mx-auto w-1/2 h-1/2 text-24 text-gray-200 animate-spin dark:text-gray-600 fill-orange"
          viewBox="0 0 100 101" fill="none" xmlns="http://www.w3.org/2000/svg">
          <path
              d="M100 50.5908C100 78.2051 77.6142 100.591 50 100.591C22.3858 100.591 0 78.2051 0 50.5908C0 22.9766 22.3858 0.59082 50 0.59082C77.6142 0.59082 100 22.9766 100 50.5908ZM9.08144 50.5908C9.08144 73.1895 27.4013 91.5094 50 91.5094C72.5987 91.5094 90.9186 73.1895 90.9186 50.5908C90.9186 27.9921 72.5987 9.67226 50 9.67226C27.4013 9.67226 9.08144 27.9921 9.08144 50.5908Z"
              fill="currentColor" />
          <path
              d="M93.9676 39.0409C96.393 38.4038 97.8624 35.9116 97.0079 33.5539C95.2932 28.8227 92.871 24.3692 89.8167 20.348C85.8452 15.1192 80.8826 10.7238 75.2124 7.41289C69.5422 4.10194 63.2754 1.94025 56.7698 1.05124C51.7666 0.367541 46.6976 0.446843 41.7345 1.27873C39.2613 1.69328 37.813 4.19778 38.4501 6.62326C39.0873 9.04874 41.5694 10.4717 44.0505 10.1071C47.8511 9.54855 51.7191 9.52689 55.5402 10.0491C60.8642 10.7766 65.9928 12.5457 70.6331 15.2552C75.2735 17.9648 79.3347 21.5619 82.5849 25.841C84.9175 28.9121 86.7997 32.2913 88.1811 35.8758C89.083 38.2158 91.5421 39.6781 93.9676 39.0409Z"
              fill="currentFill" />
      </svg>
  </div>
      `;
  } else {
    let body = document.querySelector("body");
    body.innerHTML += `
      <div role="status" id="activityIndicator"
      class="w-screen bg-gray-300 bg-opacity-80 z-50 float fixed top-0 right-0 h-screen flex items-center justify-around">
      <!-- <svg aria-hidden="true" class="w-8 h-[60vh] text-24 text-gray-200 animate-spin dark:text-gray-600 fill-primary" -->
  
  
      <svg aria-hidden="true"
          class="mx-auto w-[15vw] h-[20vh] text-24 text-gray-200 animate-spin dark:text-gray-600 fill-primary"
          viewBox="0 0 100 101" fill="none" xmlns="http://www.w3.org/2000/svg">
          <path
              d="M100 50.5908C100 78.2051 77.6142 100.591 50 100.591C22.3858 100.591 0 78.2051 0 50.5908C0 22.9766 22.3858 0.59082 50 0.59082C77.6142 0.59082 100 22.9766 100 50.5908ZM9.08144 50.5908C9.08144 73.1895 27.4013 91.5094 50 91.5094C72.5987 91.5094 90.9186 73.1895 90.9186 50.5908C90.9186 27.9921 72.5987 9.67226 50 9.67226C27.4013 9.67226 9.08144 27.9921 9.08144 50.5908Z"
              fill="currentColor" />
          <path
              d="M93.9676 39.0409C96.393 38.4038 97.8624 35.9116 97.0079 33.5539C95.2932 28.8227 92.871 24.3692 89.8167 20.348C85.8452 15.1192 80.8826 10.7238 75.2124 7.41289C69.5422 4.10194 63.2754 1.94025 56.7698 1.05124C51.7666 0.367541 46.6976 0.446843 41.7345 1.27873C39.2613 1.69328 37.813 4.19778 38.4501 6.62326C39.0873 9.04874 41.5694 10.4717 44.0505 10.1071C47.8511 9.54855 51.7191 9.52689 55.5402 10.0491C60.8642 10.7766 65.9928 12.5457 70.6331 15.2552C75.2735 17.9648 79.3347 21.5619 82.5849 25.841C84.9175 28.9121 86.7997 32.2913 88.1811 35.8758C89.083 38.2158 91.5421 39.6781 93.9676 39.0409Z"
              fill="currentFill" />
      </svg>
  </div>
      `;
  }
  setTimeout(() => {
    removeActivityIndicatorForPage();
    console.log("activity indicator removed");
  }, duration * 1000);
}

function removeActivityIndicatorForPage() {
  const element = document.getElementById("activityIndicator");
  element.remove();
}

function addActivityIndicatorToElement(elementId, duration = 6) {
  var element = document.getElementById(elementId);
  var elementContent = element.innerHTML;
  let loaderIdentifier = elementId + "ActivityIndicator";

  element.innerHTML = `
  <svg id='${loaderIdentifier}' aria-hidden="true" role="status" class="inline w-4 h-4 me-1 text-white animate-spin" viewBox="0 0 100 101" fill="none" xmlns="http://www.w3.org/2000/svg">
  <path d="M100 50.5908C100 78.2051 77.6142 100.591 50 100.591C22.3858 100.591 0 78.2051 0 50.5908C0 22.9766 22.3858 0.59082 50 0.59082C77.6142 0.59082 100 22.9766 100 50.5908ZM9.08144 50.5908C9.08144 73.1895 27.4013 91.5094 50 91.5094C72.5987 91.5094 90.9186 73.1895 90.9186 50.5908C90.9186 27.9921 72.5987 9.67226 50 9.67226C27.4013 9.67226 9.08144 27.9921 9.08144 50.5908Z" fill="#E5E7EB"/>
  <path d="M93.9676 39.0409C96.393 38.4038 97.8624 35.9116 97.0079 33.5539C95.2932 28.8227 92.871 24.3692 89.8167 20.348C85.8452 15.1192 80.8826 10.7238 75.2124 7.41289C69.5422 4.10194 63.2754 1.94025 56.7698 1.05124C51.7666 0.367541 46.6976 0.446843 41.7345 1.27873C39.2613 1.69328 37.813 4.19778 38.4501 6.62326C39.0873 9.04874 41.5694 10.4717 44.0505 10.1071C47.8511 9.54855 51.7191 9.52689 55.5402 10.0491C60.8642 10.7766 65.9928 12.5457 70.6331 15.2552C75.2735 17.9648 79.3347 21.5619 82.5849 25.841C84.9175 28.9121 86.7997 32.2913 88.1811 35.8758C89.083 38.2158 91.5421 39.6781 93.9676 39.0409Z" fill="currentColor"/>
  </svg>
  ${elementContent}
  `;

  setTimeout(() => {
    removeActivityIndicatorForElement(elementId);
    console.log("activity indicator removed");
  }, duration * 1000);
}

function removeActivityIndicatorForElement(elementId) {
  var element = document.getElementById(elementId + "ActivityIndicator");
  if (element != null || element.trim() != "") {
    element.remove();
  } else {
    console.log("invalid element id || element does not have a loader");
  }
}

////////////////// ACTIVITY INDICATOR SECTION ENDS/////////////////
