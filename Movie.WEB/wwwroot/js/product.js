var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Product/GetAll"
        },
        "columns": [
            { "data": "title", "width": "15%" },
            { "data": "rating", "width": "10%" },
            { "data": "director", "width": "15%" },
            { "data": "price", "width": "10%" },
            { "data": "genre.name", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div style="height: 60px;" class="w-75 btn-group" role="group">
                        <a href="/Admin/Product/Upsert?id=${data}"
                        class="btn btn-sm btn-outline-info mx-2" style="border-radius: 8px; height=50px;"> <i class="fa-solid fa-pen-to-square"></i> Edit</a>
                        <a onClick=Delete('/Admin/Product/Delete/${data}')
                        class="btn btn-sm btn-outline-danger mx-2" style="border-radius: 8px; height=50px;"> <i class="fa-solid fa-trash-can"></i> Delete</a>
					</div>
                        `
                },
                "width": "15%"
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}