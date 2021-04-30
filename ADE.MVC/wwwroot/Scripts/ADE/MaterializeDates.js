
export let TimePickerinstances;
export let DatePickerinstances;

export function CreateDatePicker() {
    let eventDates = [
        new Date('2020,04,1').toDateString(),
        new Date('2020,04,2').toDateString()
    ];
    let disableListDate = [
        new Date('2020,04,3').toDateString(),
        new Date('2020,04,4').toDateString()
    ];

    let optionsDate = {
        events: eventDates,
        disableWeekends: false,
        autoClose: false,
        showDaysInNextAndPreviousMonths: false,
        showClearBtn: true,
        i18n: {
            months: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
            monthsShort: ['Jan', 'Fev', 'Mar', 'Abril', 'Maio', 'Jun', 'Jul', 'Agos', 'Set', 'Out', 'Nov', 'Dez'],
            weekdays: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'],
            weekdaysShort: ['Dom', 'Seg', 'Ter', 'Quar', 'Quin', 'Sex', 'Sáb'],
            clear: "Limpar",
            cancel: "Cancelar",

        },
        format: 'dd-mm-yyyy',
        onSelect() {
            if (optionsDate.events.includes(this.date.toDateString())) {
                alert('Event Date');
            }
        },
        defaultDate : new Date(),
        disableDayFn(date) {
            if (disableListDate.includes(date.toDateString()))
                return true;
            else
                return false;
        }
    };
    let elems = document.querySelector('.datepicker');
    DatePickerinstances = M.Datepicker.init(elems, optionsDate);
};

export function CreateTimePicker() {
    var elems = document.querySelectorAll('.timepicker');

    var options = {
        twelveHour: true,
        i18n: {
            cancel: 'Cancelar',
            clear: 'Limpar',
            done: 'Ok'
        }
    };

    TimePickerinstances = M.Timepicker.init(elems, options);
};
