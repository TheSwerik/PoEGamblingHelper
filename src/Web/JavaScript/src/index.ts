import Chart from 'chart.js/auto';

// noinspection JSUnusedGlobalSymbols
export function showChart(data: number[]) {
    const ctx = document.getElementById('chart');

    // @ts-ignore
    new Chart(ctx, {
        type: 'bar',
        data: {
            labels: ['Red', 'Blue', 'Yellow', 'Green', 'Purple', 'Orange'],
            datasets: [{
                label: '# of Votes',
                data: data,
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });
}
