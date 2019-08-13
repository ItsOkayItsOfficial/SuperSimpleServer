package supersimpleserver

import (
	"io/ioutil"
	"log"
	"net/http"
)

// Page type for constructing pages to serve
type Page struct {
	Title string
	Body  []byte
}

// Page builder using the 'Page' type to output file with name/title and body
func (page *Page) save() error {
	filename := page.Title + ".txt"
	return ioutil.WriteFile(filename, page.Body, 0600)
}

// Entry into program
func supersimpleserver() {
	http.HandleFunc("/", page)
	log.Fatal(http.ListenAndServe(":9090", nill))
}
