import Vue from "vue"
function initDirective() {
    Vue.directive('enter', {
        bind: function (el, binding) {
            el.addEventListener("keyup", function (event) {
                if (event.keyCode === 13) {
                    binding.value();
				}
            });            
        }
    });
    Vue.directive('focus', {        
        inserted: function (el) {            
            el.focus();
        }
    });
}
export { initDirective }