    /**
     * Append items to an array.
     * 
     * This function appends the specified items to the end of the provided array.
     * 
     * @param {Array} arrayToBeAppended - The array to which items will be appended.
     * @param {...*} items - The items to be appended to the array.
     * @returns {void}
     */
    function addItemsToArray(arrayToBeAppended = [], ...items) {
        if (!Array.isArray(arrayToBeAppended)) {
          console.error("The first argument must be an array.");
          return;
        }
        items.forEach(item => {
          arrayToBeAppended.push(item);
        });
        console.log(arrayToBeAppended);
      }
  console.log("hello")
      const ctx = document.getElementById('myChart');
  
      let labels = [];
      let actualData = [12, 19, 3, 5, 2, 3];
      backgroundColors=["#ff8200", "#ff9b33", "#ffb466", "#ffcd99", "#ffe6cc", "#fff3e6"];
  
  
  
  
      
      let data = {
        labels: labels,
        datasets: [{
          data: actualData,
          // borderColor: "rgb(66,135,245)",
          backgroundColor:backgroundColors,
          // fill:true,
          // label: '# of Votes',
          // borderWidth: 1,
        }]
      };
  
      let configuration = {
        type: 'pie',
        data: data,
        options: {
          plugins: {
            legend: {
              display: false,
              position: "left"
            }
          },
          cutout: "50%",
          title: {
            display: true,
            text: "AGM Shareholdings"
          }
          // scales: {
          //   y: {
          //     beginAtZero: true
          //   }
          // }
        }
      }
  
      new Chart(ctx, configuration);