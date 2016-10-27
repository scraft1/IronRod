// Write your Javascript code.

var takenVerses = []; 
var selectedVerses = []; 

function selectVerse(verse, id){
    if(selectedVerses.includes(id)){
        var ind = selectedVerses.indexOf(id);
        selectedVerses.splice(ind, 1); 
        verse.classList.remove("selected"); 
    } else {
        selectedVerses.push(id);
        verse.classList.add("selected");
    }

    if(selectedVerses.length > 0){
        $(".btn-select").text("Select: "+selectedVerses.length);
        $(".btn-select").show();
    }
    else $(".btn-select").hide();
}

function createPassage(){
    var chapter = 'chapter='+document.getElementById("chapter_title").textContent; 
    var verses = 'verses='+selectedVerses.join(); 
    window.location = location.origin+'/Passages/Create?'+chapter+'&'+verses; 
}

function selectSMSet(smset){
    window.location = location.origin+'/Passages/CreateSM?smset='+smset; 
}
