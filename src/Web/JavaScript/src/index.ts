import Chart from 'chart.js/auto';

// noinspection JSUnusedGlobalSymbols
export function showChart(data: number[]) {
    const ctx = document.getElementById('chart');
    console.log(ctx)

    // @ts-ignore
    new Chart(ctx, {
        type: 'pie',
        data: {
            labels: ['Worst', 'Middle', 'Best'],
            datasets: [{
                label: 'Outcomes',
                data: data,
                backgroundColor: [
                    'rgb(255, 50, 50)',
                    'rgb(255, 255, 50)',
                    'rgb(50, 255, 50)'
                ],
            }]
        }
    });
}
