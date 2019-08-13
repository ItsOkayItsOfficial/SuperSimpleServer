package supersimpleserver

import (
	"fmt"
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

// Page loader finds and reads requested page into memory
func loadPage(title string) (*Page, error) {
	filename := title + "*"
	body, err := ioutil.ReadFile(filename)
	if err != nil {
		return nil, err
	}
	return &Page{Title: title, Body: body}, nil
}

// Page server/viewer loads page and defines display of it
func viewPage(response http.ResponseWriter, request *http.Request) {
	title := request.URL.Path[len("/"):]
	page, _ := loadPage(title)
	fmt.Fprintf(response, "<div><h1>%s</h1></div><div><p>%s</p></div>", page.Title, page.Body)
}

// Entry into program
func supersimpleserver() {
	http.HandleFunc("/", loadPage)
	log.Fatal(http.ListenAndServe(":9090", nil))
}
