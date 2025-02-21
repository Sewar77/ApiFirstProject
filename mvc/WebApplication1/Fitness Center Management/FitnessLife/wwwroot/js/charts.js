// admin-charts.js

// Function to initialize the charts
function initializeCharts(totalRevenue, activeMembers, newMembersThisMonth, newMembersThisYear) {
    // Worldwide Sales Chart
    var ctx1 = document.getElementById('worldwide-sales').getContext('2d');
    var worldwideSalesChart = new Chart(ctx1, {
        type: 'line', // Line chart to visualize sales trends
        data: {
            labels: ['January', 'February', 'March', 'April', 'May'], // Example months
            datasets: [{
                label: 'Sales Revenue',
                data: [1000, 1200, 1400, 1300, totalRevenue], // Replace with actual data
                backgroundColor: 'rgba(0, 123, 255, 0.2)',
                borderColor: 'rgba(0, 123, 255, 1)',
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });

    // Sales & Revenue Chart
    var ctx2 = document.getElementById('salse-revenue').getContext('2d');
    var salesRevenueChart = new Chart(ctx2, {
        type: 'bar', // Bar chart for members and revenue
        data: {
            labels: ['Active Members', 'New Members This Month', 'New Members This Year'],
            datasets: [{
                label: 'Members & Revenue Stats',
                data: [ActiveMembers, newMembersThisMonth, NewMembersThisYear], // Replace with actual data
                backgroundColor: ['rgba(0, 123, 255, 0.2)', 'rgba(40, 167, 69, 0.2)', 'rgba(220, 53, 69, 0.2)'],
                borderColor: ['rgba(0, 123, 255, 1)', 'rgba(40, 167, 69, 1)', 'rgba(220, 53, 69, 1)'],
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });
}
