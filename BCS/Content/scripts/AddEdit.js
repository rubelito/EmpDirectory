$(document).ready(function () {
    $('#Country').on("change", function(){
        var selectedCountry = this.value;
        HideOrShowStateDropdown(selectedCountry, true);
    });

    $('#StateDropDown').on("change", function(){
        var selectedState = this.value;
        $('#State').val(selectedState);
    });

    function PopulateStatesOnLoad()
    {
        var initialCountryValue = $('#Country').val();
        HideOrShowStateDropdown(initialCountryValue, false);
    }

    function HideOrShowStateDropdown(country, clearState)
    {
        if (country == 'United States of America' || country == 'Philippines')
        {
            $('#State').hide();
            $('#StateDropDown').show();
            LoadStates(country);
        }
        else
        {
            $('#State').show();
            $('#StateDropDown').hide();
        }

        if (clearState == true)
        {
            $('#State').val('');
        }

    }

    var states = [];
    function LoadStates(countryName)
    {
        var countryRequest = {country : countryName};
        $.ajax({
            type: "Post",
            url: "/Employees/GetStates",
            data: JSON.stringify(countryRequest),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function(data){
                document.getElementById("StateDropDown").options.length = 0;
                states = data;
                for (var i = 0; i <= states.length -1; i++) {
                    var name = states[i].Name;
                    $('#StateDropDown').append('<option value="' + name +'">' + name + '</option>');
                }
                SetSelectedState();
            }
        });
    }

    function SetSelectedState()
    {
        var currentState = $('#State').val();
        var stateExistedOntheList = false;
        for (var i = 0; i <= states.length -1; i++) {
            var name = states[i].Name;
            if (name == currentState){
                stateExistedOntheList = true;
                break;
            }
        }

        if (stateExistedOntheList == true)
        {
            $('#StateDropDown').val(currentState).prop('selected', true);
        }
        else
        {
            $('#State').val($('#StateDropDown').val());
        }
    }
    PopulateStatesOnLoad();
});