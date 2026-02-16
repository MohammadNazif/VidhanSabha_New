$(document).ready(function () {
    const urlParams = new URLSearchParams(window.location.search);

    // Get values from URL
    const casteIds = urlParams.get('casteIds') ? urlParams.get('casteIds').split(',') : [];
    const desgIds = urlParams.get('desgIds') ? urlParams.get('desgIds').split(',') : [];
    const occuIds = urlParams.get('occuIds') ? urlParams.get('occuIds').split(',') : [];

    // Set selected values in dropdowns
    $('#CasteFilter').val(casteIds).trigger('change');
    $('#desigFilter').val(desgIds).trigger('change');
    $('#OccupationFilter').val(occuIds).trigger('change');

    // Initialize Select2
    $('#CasteFilter').select2({ placeholder: "Select Caste" });
    $('#desigFilter').select2({ placeholder: "Select Designation" });
    $('#OccupationFilter').select2({ placeholder: "Select Occupation" });

    // Filter button logic
    $('#filterButton').on('click', function () {
        var selectedCaste = $('#CasteFilter').val(); // caste
        var selectedDesig = $('#desigFilter').val();     // designation
        var selectedOccu = $('#OccupationFilter').val(); // occupation

        if ((!selectedCaste || selectedCaste.length === 0) &&
            (!selectedDesig || selectedDesig.length === 0) &&
            (!selectedOccu || selectedOccu.length === 0)) {
            Swal.fire({
                icon: 'warning',
                title: 'No filter selected',
                text: 'Please select at least one filter.',
                confirmButtonText: 'OK'
            });
            return;
        }

        // Build query string
        let query = [];
        if (selectedCaste && selectedCaste.length > 0) {
            query.push(`casteIds=${selectedCaste.join(',')}`);
        }
        if (selectedDesig && selectedDesig.length > 0) {
            query.push(`desgIds=${selectedDesig.join(',')}`);
        }
        if (selectedOccu && selectedOccu.length > 0) {
            query.push(`occuIds=${selectedOccu.join(',')}`);
        }

        const url = `${window.location.pathname}?${query.join('&')}`;
        window.location.href = url;
    });

    // Reset filter logic
    $('#resetFilters').click(function () {
        $('#CasteFilter').val(null).trigger('change');
        $('#desigFilter').val(null).trigger('change');
        $('#OccupationFilter').val(null).trigger('change');
        window.location.href = window.location.pathname;
    });
});