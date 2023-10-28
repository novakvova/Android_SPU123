package com.example.mystore.network;

import com.example.mystore.dto.CategoryCreateDTO;
import com.example.mystore.dto.CategoryItemDTO;

import java.util.List;

import okhttp3.MultipartBody;
import okhttp3.RequestBody;
import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.GET;
import retrofit2.http.Multipart;
import retrofit2.http.POST;
import retrofit2.http.Part;

public interface CategoriesApi {
    @Multipart
    @POST("/api/categories/create")
    public Call<CategoryItemDTO> create(@Part MultipartBody.Part image,
                                        @Part RequestBody name,
                                        @Part RequestBody description);


    @GET("/api/categories/list")
    public Call<List<CategoryItemDTO>> list();
}
